using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Forwarder.Dialogs
{
    class Dialog
    {
        static public Window WINDOW;

        static public void ShowError(String errorText, String windowHeader)
        {
            WINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                ErrorMessageDialog errorMessageDialog = new ErrorMessageDialog();
                errorMessageDialog.Title = windowHeader;
                errorMessageDialog.errorText.Text = errorText;
                errorMessageDialog.Show();
            }));
        }

        static public void ShowDialogError(String errorText, String windowHeader)
        {
            WINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                ErrorMessageDialog errorMessageDialog = new ErrorMessageDialog();
                errorMessageDialog.Title = windowHeader;
                errorMessageDialog.errorText.Text = errorText;
                errorMessageDialog.ShowDialog();
            }));
        }

        static public void ShowInformation(String headerInformation, String textInformation, String windowHeader)
        {
            WINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                InformationMessageDialog informationMessageDialog = new InformationMessageDialog();
                informationMessageDialog.Title = windowHeader;
                informationMessageDialog.headerMessage.Text = headerInformation;
                informationMessageDialog.text.Text = textInformation;
                informationMessageDialog.Show();
            }));
        }

        static public void ShowWarming(String headerInformation, String textInformation, String windowHeader)
        {
            WINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                WarningMessageDialog warningMessageDialog = new WarningMessageDialog();
                warningMessageDialog.Title = windowHeader;
                warningMessageDialog.headerMessage.Text = headerInformation;
                warningMessageDialog.text.Text = textInformation;
                warningMessageDialog.Show();
            }));

        }

        static public bool ShowYesNoDialog(String headerInformation, String textInformation, String windowHeader)
        {
            bool result = false;
            WINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                YesNoMessageDialog yesNoMessageDialog = new YesNoMessageDialog();
                yesNoMessageDialog.Title = windowHeader;
                yesNoMessageDialog.headerMessage.Text = headerInformation;
                yesNoMessageDialog.text.Text = textInformation;
                yesNoMessageDialog.ShowDialog();
                result = yesNoMessageDialog.Result;
            }));
            return result;
        }
    }
}
