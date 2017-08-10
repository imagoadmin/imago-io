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
//      2. Query the list of projects and datasets available to the user,
//      3. Query the list of data entities associated data items, and
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
                MessageBox.Show("Unable to get the account's list of projects and datasets.");
                panelDisplay.Visible = false;
                showProgress.Visible = false;
                return;
            }
            UserContext context = result.Value;

            lstProjects.DisplayMember = "Name";
            lstProjects.DataSource = context.Projects;

            panelDisplay.Visible = true;
            showProgress.Visible = false;
        }

        //
        //  Update the dropdown list of datasets when the selected project changes
        //

        private void lstProjects_SelectedValueChanged(object sender, EventArgs e)
        {
            lstDatasets.DisplayMember = "Name";
            lstDatasets.DataSource = (lstProjects.SelectedValue as Project).Datasets;
        }

        //
        //  Update the dropdown list of data series types when the selected dataset changes.
        //  It also downloads and updates the list of data entities in the dataset.
        //

        private async void lstDatasets_SelectedValueChanged(object sender, EventArgs e)
        {
            lstDataSeriesTypes.DisplayMember = "Name";
            lstDataSeriesTypes.DataSource = (lstDatasets.SelectedValue as Dataset).DataSeriesTypes;

            lstDataEntity.DisplayMember = "Name";

            Dataset dataset = (Dataset)lstDatasets.SelectedValue;
            if (dataset != null)
            {
                showProgress.Visible = true;
                Client.DataEntityQueryParameters query = new Client.DataEntityQueryParameters { datasetId = dataset.Id };
                Result<List<DataEntity>> result = await connection.SearchForDataEntity(query, cancelToken);
                showProgress.Visible = false;

                if (result.Code != ResultCode.ok || result.Value.Count == 0)
                {
                    MessageBox.Show("Unable to find any matching data entities ine the selected dataset.");
                    return;
                }
                lstDataEntity.DataSource = result.Value;
            }
            else
            {
                lstDataEntity.DataSource = null;
            }
        }

        //
        //  Update the dropdown list of imagery types when the selected data series type changes
        //

        private void lstDataSeriesTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            lstImageryTypes.DisplayMember = "Name";
            lstImageryTypes.DataSource = (lstDataSeriesTypes.SelectedValue as DataSeriesType).ImageryTypes;
        }

        //
        //  Download and update the list of data items associated with the selected data entity
        //

        private async void lstDataEntity_SelectedValueChanged(object sender, EventArgs e)
        {
            DataEntity dataEntity = lstDataEntity.SelectedValue as DataEntity;
            DataSeriesType dataSeriesType = (DataSeriesType)lstDataSeriesTypes.SelectedValue;

            if (dataEntity != null && dataSeriesType != null)
            {
                showProgress.Visible = true;
                Client.DataItemQueryParameters query = new Client.DataItemQueryParameters { dataEntityId = dataEntity.Id, dataSeriesTypeId = dataSeriesType.Id };
                Result<List<DataItem>> result = await connection.SearchForDataItem(query, cancelToken);
                showProgress.Visible = false;
                if (result.Code != ResultCode.ok || result.Value.Count == 0)
                {
                    MessageBox.Show("The specified data entity does not have any data items.");
                    return;
                }
                lstDataItems.DataSource = result.Value;
            }
            else
            {
                lstDataItems.DataSource = null;
            }
        }

        //
        //  Download and display the image for the selected data item and imagery type
        //

        private async void butDisplay_Click(object sender, EventArgs e)
        {
            DataItem dataItem = lstDataItems.SelectedValue as DataItem;
            ImageryType imageryType = lstImageryTypes.SelectedValue as ImageryType;
            if (dataItem == null || imageryType == null)
            {
                MessageBox.Show("A data item and imagery type must be selected first.");
                return;
            }

            showProgress.Visible = true;
            Client.ImageryQueryParameters query = new Client.ImageryQueryParameters { dataItemId = dataItem.Id, imageryTypeId = imageryType.Id };
            string tempFileName = Path.GetTempFileName();
            Result<string> result = await connection.DownloadImageryToFile(query, tempFileName, cancelToken);
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
