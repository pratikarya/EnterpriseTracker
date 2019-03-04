using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Logging;
using System;
using System.Collections.Generic;

namespace EnterpriseTracker.Droid.Adapters
{
    public class CommonListAdapter<T> : BaseAdapter<T>
    {
        public List<T> data;
        private Context Context;

        public CommonListAdapter(Context context, List<T> list)
        {
            data = list;
            Context = context;
        }
        public override T this[int position]
        {
            get
            {
                return data[position];
            }
        }
        public override int Count
        {
            get
            {
                return data.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(Context).Inflate(Resource.Layout.commonlist_cellview, null, false);
                }
                TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
                txtName.Text = data[position].ToString();
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
            }
            return row;
        }
    }
}