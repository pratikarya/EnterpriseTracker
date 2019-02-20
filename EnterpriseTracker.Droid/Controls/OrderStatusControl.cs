using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Localization;
using System.Linq;
using EnterpriseTracker.Droid.Utility;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using System;

namespace EnterpriseTracker.Droid.Controls
{
    public class OrderStatusControl : LinearLayout
    {
        string Text;
        public OrderStatus SelectedStatus { get; set; }
        public EventHandler StatusChanged;
        List<RadioButton> _buttonList = new List<RadioButton>();
        TextView Label;
        private bool _isReview;

        public OrderStatusControl(Context context, OrderStatus selectedStatus, string text) : base(context)
        {
            SelectedStatus = selectedStatus;
            Text = text;
            Orientation = Orientation.Vertical;
            LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            {
                Gravity = GravityFlags.Left,
                BottomMargin = AndroidHelper.ConvertDpToPx(10),
                RightMargin = AndroidHelper.ConvertDpToPx(10),
                LeftMargin = AndroidHelper.ConvertDpToPx(10),
            };

            Label = new TextView(context);
            Label.TextSize = 18;
            Label.SetTextColor(Resources.GetColor(Resource.Color.highlightColor));
            Label.LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            {
                TopMargin = AndroidHelper.ConvertDpToPx(5),
                BottomMargin = AndroidHelper.ConvertDpToPx(5)
            };
            
            AddView(Label);

            PopulateControl();
        }

        public void Update(OrderStatus status)
        {
            SelectedStatus = status;
            ClearItems();
            PopulateControl();
        }

        private void PopulateControl()
        {
            Label.Text = Text;
            var list = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var radioButton = new RadioButton(Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                    {
                        BottomMargin = 5
                    },
                    TextSize = 14
                };
                radioButton.SetText(list[i].ToString(), TextView.BufferType.Normal);
                radioButton.Tag = i;
                radioButton.Checked = false;
                radioButton.Selected = false;
                radioButton.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

                _buttonList.Add(radioButton);

                AddView(radioButton);

                if (SelectedStatus == list[i])
                {
                    radioButton.Selected = true;
                    radioButton.Checked = true;
                }
                radioButton.CheckedChange += (sender, e) =>
                {
                    var radio = sender as RadioButton;

                    if (radio.Checked)
                    {
                        foreach (var button in _buttonList)
                        {
                            if (button == radio)
                                continue;

                            if (button.Checked)
                            {
                                button.Checked = false;
                                break;
                            }
                        }
                    }

                    SelectedStatus = list.FirstOrDefault(x => x.ToString() == radio.Text);
                    StatusChanged?.Invoke(this, null);
                };
            }
        }

        private void ClearItems()
        {
            foreach (var radio in _buttonList)
            {
                RemoveView(radio);
            }

            _buttonList.Clear();
        }
    }
}