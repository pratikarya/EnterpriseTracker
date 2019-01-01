using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.Message;
using EnterpriseTracker.Droid.Fragments;
using EnterpriseTracker.Droid.Utility;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace EnterpriseTracker.Droid.Controls
{
    public delegate void ValueSelectedDelegate<T>(T value);

    public class DropdownControl<T> : LinearLayout
    {
        Activity _activity;
        TextView _txtName;
        ImageView _ivDropdown;
        List<T> _data;
        string _placeholder;

        private T _selectedData;
        public T SelectedData
        {
            get { return _selectedData; }
            set
            {
                _selectedData = value;
                OnValueSelected?.Invoke(SelectedData);
                _txtName.Text = _selectedData.ToString();
                _txtName.SetTextColor(AndroidHelper.CurrentTheme.TextColor);
            }
        }

        CommonListFrgament<T> _frag { get; set; }
        IMvxMessenger Messenger { get; set; }
        MvxSubscriptionToken _valueSelectedToken { get; set; }
        public bool IsEnabled { get; set; }
        public event ValueSelectedDelegate<T> OnValueSelected;

        public DropdownControl(Activity activity, string placeholder) : base(activity)
        {
            _activity = activity;
            _placeholder = placeholder;
            
            Messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            _valueSelectedToken = Messenger.Subscribe<ValueSelectedMessage<T>>(ValueSelected);

            CreateUi();
        }

        public void SetData(List<T> data)
        {
            _data = data;
        }

        private void CreateUi()
        {
            Orientation = Orientation.Vertical;
            SetGravity(GravityFlags.CenterVertical);
            SetPadding(5, 5, 5, 5);
            LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            {
                BottomMargin = AndroidHelper.ConvertDpToPx(10)
            };

            var ll = new LinearLayout(_activity)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                {
                    BottomMargin = AndroidHelper.ConvertDpToPx(5)
                }
            };

            _txtName = new TextView(_activity);
            _txtName.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent) { RightMargin = 5,  Weight = 1 };
            _txtName.Gravity = GravityFlags.Left;
            _txtName.Text = "Select " + _placeholder;
            _txtName.SetTextColor(AndroidHelper.CurrentTheme.ColorPlaceHolder);
            _txtName.TextSize = AndroidHelper.CurrentTheme.TextSize;

            _ivDropdown = new ImageView(_activity);
            _ivDropdown.LayoutParameters = new LinearLayout.LayoutParams(AndroidHelper.ConvertDpToPx(15), AndroidHelper.ConvertDpToPx(15));
            _ivDropdown.SetImageResource(Resource.Drawable.ic_arrow_down);

            ll.AddView(_txtName);
            ll.AddView(_ivDropdown);

            AddView(ll);
            AddView(new SeperatorControl(_activity)); 
            this.Click += DropdownControl_Click;
        }

        private void ValueSelected(ValueSelectedMessage<T> message)
        {
            if (message != null && message.SelectedValue != null)
            {
                SelectedData = message.SelectedValue;
            }
        }
         
        private void DropdownControl_Click(object sender, EventArgs e)
        {
            _frag = new CommonListFrgament<T>(_data, _placeholder);
            var transcation = _activity.FragmentManager.BeginTransaction();
            _frag.Show(transcation, "");
        }

        protected override void Dispose(bool disposing)
        {
            _valueSelectedToken?.Dispose();
        }
    }
}