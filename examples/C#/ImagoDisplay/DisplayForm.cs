using Imago.IO;
using Imago.IO.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

//
//  Imago Display Example
//
//  Imago Inc
//  August 2017
//
//  This example demonstrates how to:
//
//      1. Sign into imago,
//      2. Query the list of workspaces and datasets available to the user,
//      3. Query the list of collections associated imageries, and
//      4. Download and display an image.
//

namespace ImagoDisplay
{
    public partial class DisplayForm : Form
    {
        public DisplayForm()
        {
            InitializeComponent();
        }

        Client connection = null;
        CancellationToken cancelToken = new CancellationToken();
        private void DisplayForm_Load(object sender, EventArgs e)
        {
            panelDisplay.Visible = false;
        }

        //
        //  Connect to Imago and get a user's context
        //

        private async void butConnect_Click(object sender, EventArgs e)
        {
            panelDisplay.Visible = false;
            showProgress.Visible = true;

            Credentials credentials = new Credentials { UserName = txtUserName.Text, Password = txtPassword.Text };

            connection = new Client();
            bool ok = await connection.SignIn(credentials);

            if (!ok)
            {
                MessageBox.Show("The account name and password were not recognised.");
                panelDisplay.Visible = false;
                showProgress.Visible = false;
                return;
            }

            Result<UserContext> result = await connection.GetUserContext();
            if (result.Code != ResultCode.ok)
            {
                MessageBox.Show("Unable to get the account's list of workspaces and datasets.");
                panelDisplay.Visible = false;
                showProgress.Visible = false;
                return;
            }
            UserContext context = result.Value;

            lstWorkspaces.DisplayMember = "Name";
            lstWorkspaces.DataSource = context.Workspaces;

            panelDisplay.Visible = true;
            showProgress.Visible = false;
        }

        //
        //  Update the dropdown list of datasets when the selected workspace changes
        //

        private void lstWorkspaces_SelectedValueChanged(object sender, EventArgs e)
        {
            lstDatasets.DisplayMember = "Name";
            lstDatasets.DataSource = (lstWorkspaces.SelectedValue as Workspace).Datasets;
        }

        //
        //  Update the dropdown list of imagery types when the selected dataset changes.
        //  It also downloads and updates the list of collections in the dataset.
        //

        private async void lstDatasets_SelectedValueChanged(object sender, EventArgs e)
        {
            lstImageryTypes.DisplayMember = "Name";
            lstImageryTypes.DataSource = (lstDatasets.SelectedValue as Dataset).ImageryTypes;

            lstCollections.DisplayMember = "Name";

            Dataset dataset = (Dataset)lstDatasets.SelectedValue;
            if (dataset != null)
            {
                showProgress.Visible = true;
                Client.CollectionQueryParameters query = new Client.CollectionQueryParameters { datasetId = dataset.Id };
                Result<List<Collection>> result = await connection.SearchForCollection(query, cancelToken);
                showProgress.Visible = false;

                if (result.Code != ResultCode.ok || result.Value.Count == 0)
                {
                    MessageBox.Show("Unable to find any matching collections ine the selected dataset.");
                    return;
                }
                lstCollections.DataSource = result.Value;
            }
            else
            {
                lstCollections.DataSource = null;
            }
        }

        //
        //  Update the dropdown list of image types when the selected imagery type changes
        //

        private async void lstDataSeriesTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            lstImageTypes.DisplayMember = "Name";
            lstImageTypes.DataSource = (lstImageryTypes.SelectedValue as ImageryType).ImageTypes;
            await UpdateImageries();
        }

        //
        //  Download and update the list of imageries associated with the selected collection
        //

        private async void lstCollections_SelectedValueChanged(object sender, EventArgs e)
        {
            await UpdateImageries();
        }

        private async System.Threading.Tasks.Task UpdateImageries()
        {
            Collection dataEntity = lstCollections.SelectedValue as Collection;
            ImageryType dataSeriesType = (ImageryType)lstImageryTypes.SelectedValue;

            if (dataEntity != null && dataSeriesType != null)
            {
                showProgress.Visible = true;
                Client.ImageryQueryParameters query = new Client.ImageryQueryParameters { collectionId = dataEntity.Id, imageryTypeId = dataSeriesType.Id };
                Result<List<Imagery>> result = await connection.SearchForImagery(query, cancelToken);
                showProgress.Visible = false;
                if (result.Code != ResultCode.ok || result.Value.Count == 0)
                {
                    MessageBox.Show("The specified collection does not have any imageries.");
                    return;
                }
                lstImageries.DataSource = result.Value;
            }
            else
            {
                lstImageries.DataSource = null;
            }
        }

        //
        //  Download and display the image for the selected imagery and image type
        //

        private async void butDisplay_Click(object sender, EventArgs e)
        {
            Imagery dataItem = lstImageries.SelectedValue as Imagery;
            ImageType imageryType = lstImageTypes.SelectedValue as ImageType;
            if (dataItem == null || imageryType == null)
            {
                MessageBox.Show("A imagery and image type must be selected first.");
                return;
            }

            showProgress.Visible = true;
            Client.ImageQueryParameters query = new Client.ImageQueryParameters { imageryId = dataItem.Id, imageTypeId = imageryType.Id };
            string tempFileName = Path.GetTempFileName();
            Result<string> result = await connection.DownloadImageToFile(query, tempFileName, cancelToken);
            showProgress.Visible = false;

            if (result.Code != ResultCode.ok)
            {
                MessageBox.Show("Unable to find and download the selected image.");
                return;
            }
            picDisplay.ImageLocation = result.Value;
        }
    }
}
