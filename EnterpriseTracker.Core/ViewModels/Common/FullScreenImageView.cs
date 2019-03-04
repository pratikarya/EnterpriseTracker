using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.ViewModels.Common
{
    public class FullScreenImageViewModel : BaseViewModel<string>
    {
        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged(() => ImagePath);
            }
        }

        public override void PrepareImpl(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
}
