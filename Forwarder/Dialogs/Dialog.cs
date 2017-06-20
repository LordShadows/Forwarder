using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forwarder.Dialogs
{
    class Dialog
    {
        static public void ShowError(String errorText, String windowHeader)
        {
            ErrorMessageDialog errorMessageDialog = new ErrorMessageDialog();
            errorMessageDialog.Title = windowHeader;
            errorMessageDialog.errorText.Text = errorText;
            errorMessageDialog.Show();
        }

        static public void ShowInformation(String headerInformation, String textInformation, String windowHeader)
        {
            InformationMessageDialog informationMessageDialog = new InformationMessageDialog();
            informationMessageDialog.Title = windowHeader;
            informationMessageDialog.headerMessage.Text = headerInformation;
            informationMessageDialog.text.Text = textInformation;
            informationMessageDialog.Show();
        }

        static public void ShowWarming(String headerInformation, String textInformation, String windowHeader)
        {
            WarningMessageDialog warningMessageDialog = new WarningMessageDialog();
            warningMessageDialog.Title = windowHeader;
            warningMessageDialog.headerMessage.Text = headerInformation;
            warningMessageDialog.text.Text = textInformation;
            warningMessageDialog.Show();
            
        }

        static public bool ShowYesNoDialog(String headerInformation, String textInformation, String windowHeader)
        {
            YesNoMessageDialog yesNoMessageDialog = new YesNoMessageDialog();
            yesNoMessageDialog.Title = windowHeader;
            yesNoMessageDialog.headerMessage.Text = headerInformation;
            yesNoMessageDialog.text.Text = textInformation;
            yesNoMessageDialog.ShowDialog();
            return yesNoMessageDialog.Result;
        }
    }
}
