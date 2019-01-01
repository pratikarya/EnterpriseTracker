using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using EnterpriseTracker.Core.Message;
using EnterpriseTracker.Droid.Adapters;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace EnterpriseTracker.Droid.Fragments
{
    public class CommonListFrgament<T> : DialogFragment
    {
        List<T> _data { get; set; }
        List<T> _filteredData { get; set; }
        T _selectedValue { get; set; }
        string _title;

        ListView _lvValues;
        CommonListAdapter<T> _adapter;
        ImageButton _btnCancel;
        EditText _etSearch;
        TextView _txtTitle;

        IMvxMessenger Messenger { get; set; }

        public CommonListFrgament(List<T> data, string title)
        {
            _title = title;
            _data = data;
            _filteredData = data;
            Messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.common_list_view, container, false);

            _lvValues = view.FindViewById<ListView>(Resource.Id.lvValues);
            _txtTitle = view.FindViewById<TextView>(Resource.Id.txtTitle);
            _btnCancel = view.FindViewById<ImageButton>(Resource.Id.btnCancel);
            _etSearch = view.FindViewById<EditText>(Resource.Id.etSearch);

            _lvValues.Adapter = _adapter = new CommonListAdapter<T>(Activity, _filteredData);
            _txtTitle.Text = _title;
            RegisterHandlers();

            return view;
        }

        private void RegisterHandlers()
        {
            _etSearch.TextChanged += _etSearch_TextChanged;
            _btnCancel.Click += _btnCancel_Click;
            _lvValues.ItemClick += _lvValues_ItemClick;
        }

        private void UnRegisterHandlers()
        {
            _etSearch.TextChanged -= _etSearch_TextChanged;
            _btnCancel.Click -= _btnCancel_Click;
            _lvValues.ItemClick -= _lvValues_ItemClick;
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        private void _etSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var searchText = e.Text.ToString();
            if (string.IsNullOrEmpty(searchText))
            {
                _filteredData = _data;
                _adapter.NotifyDataSetChanged();
                return;
            }
            _filteredData = _data.Where(x => x.ToString().ToLower().Contains(searchText.ToLower())).ToList();
            _adapter.NotifyDataSetChanged();
        }

        private void _lvValues_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            _selectedValue = _data[e.Position];
            Messenger.Publish(new ValueSelectedMessage<T>(this, _selectedValue));
            Dismiss();
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            UnRegisterHandlers();
        }
    }
}