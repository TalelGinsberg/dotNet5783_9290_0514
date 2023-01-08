﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Orders;

/// <summary>
/// Interaction logic for Order.xaml
/// </summary>
/// 



public partial class Order : Window
{


    BlApi.IBl? bl = BlApi.Factory.Get(); // get bl from factory
    int idRec = 0;
    bool managerFunc = false;
    
    public Order(int x = 0, bool manager = false)//constructor
    {

        InitializeComponent();
        BO.OrderTracking orderT = bl.Order.TrackingOrder(x);//get order information for x
        if(manager && orderT.tuplesList!.ToList().Count()==1 )
        {
            orderInfoButton.Content = "Order information and update";
        }
        
        managerFunc = manager;

        idRec = x;
        try
        {
            status.Text = orderT.Status.ToString();
            orderId.Text = x.ToString();
            try
            {
                List<(DateTime?, string?)>? tuplelist = orderT.tuplesList!.ToList();
                int size = tuplelist.Count();
                orderingDate.Text = tuplelist[0].Item1.ToString();
                if (size > 1)
                    shippingDate.Text = tuplelist[1].Item1.ToString();
                if (size > 2) arrivalDate.Text = tuplelist[2].Item1.ToString();

                if (size < 3)
                {
                    arrivalDate.Visibility = Visibility.Collapsed;
                    labelA.Visibility = Visibility.Collapsed;
                    if (size < 2)
                    {
                        shippingDate.Visibility = Visibility.Collapsed;
                        labelS.Visibility = Visibility.Collapsed;
                        if (manager) shipOrderByManager.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (manager) delinerOrderByManager.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (BO.DoesntExistExeption ex)
            {

                string innerEx = "";
                if (ex.InnerException != null)
                    innerEx = ": " + ex.InnerException.Message;
                MessageBox.Show("unsucessfull selection:" + ex.Message + innerEx); // for user print exception
                
            }
            


        }
        catch (BO.DoesntExistExeption ex)
        {
            string innerEx = "";
            if (ex.InnerException != null)
                innerEx = ": " + ex.InnerException.Message;
            MessageBox.Show("unsucessfull selection:" + ex.Message + innerEx); // for user print exception
        }     


    }

    private void orderInfoButton_Click(object sender, RoutedEventArgs e)
    {
        
        new PL.Orders.OrderInfo(idRec, managerFunc).ShowDialog();

        try
        {

            BO.OrderTracking orderT = bl!.Order.TrackingOrder(idRec);
        }
        catch(BO.DoesntExistExeption)
        {
            orderInfoButton.Visibility = Visibility.Collapsed;
            orderDeleted.Visibility = Visibility.Visible;
            labelO.Visibility = Visibility.Collapsed;
            orderingDate.Visibility = Visibility.Collapsed;
            shipOrderByManager.Visibility = Visibility.Collapsed;
        }
        catch(BO.InvalidInputExeption ex)
        {
            string innerEx = "";
            if (ex.InnerException != null)
                innerEx = ": " + ex.InnerException.Message;
            MessageBox.Show("unsucessfull selection:" + ex.Message + innerEx); // for user print exception
        }
    }

    private void shipOrderByManager_Click(object sender, RoutedEventArgs e)
    {
        bl!.Order.UpdateShip(idRec);
        shipOrderByManager.Visibility = Visibility.Collapsed;
        try
        {
            List<(DateTime?, string?)>? tuplelist = bl.Order.TrackingOrder(idRec).tuplesList!.ToList();

            shippingDate.Text = tuplelist[1].Item1.ToString();
            shippingDate.Visibility = Visibility.Visible;
            labelS.Visibility = Visibility.Visible;
            delinerOrderByManager.Visibility = Visibility.Visible;
            orderInfoButton.Content = "Order information";
            status.Text = "Shipped";
        }
        catch(BO.DoesntExistExeption ex)
        {

            string innerEx = "";
            if (ex.InnerException != null)
                innerEx = ": " + ex.InnerException.Message;
            MessageBox.Show("unsucessfull selection:" + ex.Message + innerEx); // for user print exception
        }
    }

    private void delinerOrderByManager_Click(object sender, RoutedEventArgs e)
    {

        bl!.Order.UpdateDelivery(idRec);
        delinerOrderByManager.Visibility = Visibility.Collapsed;
        try
        {
            List<(DateTime?, string?)>? tuplelist = bl.Order.TrackingOrder(idRec).tuplesList!.ToList();

            arrivalDate.Text = tuplelist[2].Item1.ToString();
            arrivalDate.Visibility = Visibility.Visible;
            labelA.Visibility = Visibility.Visible;
            status.Text = "Arrived";
        }
        catch(BO.DoesntExistExeption ex)
        {

            string innerEx = "";
            if (ex.InnerException != null)
                innerEx = ": " + ex.InnerException.Message;
            MessageBox.Show("unsucessfull selection:" + ex.Message + innerEx); // for user print exception
        }
    }
}