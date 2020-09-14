using ImageCompression.SelectDialog;

namespace ImageCompression.Helpers
{
    public static class SelectDialogHelper
    {
        public static string SelectFolder()
        {
            var select = new Select
            {
                InitialFolder = "C:\\"
            };
            select.ShowDialog();

            return select.Folder;
        }
    }
}