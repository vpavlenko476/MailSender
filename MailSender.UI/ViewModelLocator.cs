using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MailSender.UI.ViewModel;

namespace MailSender.UI
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

            if (ViewModelBase.IsInDesignModeStatic)
            {

            }
            else
            {

            }

            SimpleIoc.Default.Register<MailSenderVM>();            
        }

        public MailSenderVM MailSenderVM
        {
            get { return SimpleIoc.Default.GetInstance<MailSenderVM>(); }
        }
       
    }
}
