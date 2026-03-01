using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using test_demo.Models;

namespace test_demo;

public partial class CatalogWindow : Window
{

    private int _currentUserId;
    public CatalogWindow()
    {
        InitializeComponent();
        LoadBox();
        Get();

    }

    public CatalogWindow(int userid)
    {
        InitializeComponent();
        _currentUserId = userid;
        CheckRole();
        LoadBox();
        Get();


    }

    private void CheckRole()
    {
        using var context = new DemoContext();

        var curUser = context.Users.Where(x => x.Userid == _currentUserId).FirstOrDefault();
        if (curUser.Userrole == 2 || curUser.Userrole == 3)

        {
            AddButton.IsVisible = true;
        }
    }

    private void LoadBox()
    {
        using var context = new DemoContext();

        var manufacturer = context.Manufacturers.Select(x => x.Manufacturername).ToList();

        manufacturer.Add("Все производители");

        Filter.ItemsSource = manufacturer.OrderByDescending(x => x == "Все производители");

        Filter.SelectedIndex = 0;


    }

    private void Get()
    {
        using var context = new DemoContext();

        var allProducts = context.Products.Include(x => x.ProductcategoryNavigation).Include(x => x.ProductmanufacturerNavigation).ToList();

        switch (Sort.SelectedIndex)
        {
            case 0:
                allProducts = allProducts.OrderBy(x => x.Productcost).ToList();
                break;
            case 1:
                allProducts = allProducts.OrderByDescending(x => x.Productcost).ToList();
                break;
        }

        if (Filter.SelectedItem != null && Filter.SelectedItem.ToString() != "Все производители")
        {
            allProducts = allProducts.Where(x => x.ProductmanufacturerNavigation.Manufacturername == Filter.SelectedItem.ToString()).ToList();
        }




        if (SearchBox.Text != null)
        {
            var searchText = SearchBox.Text.ToLower();
            allProducts = allProducts.Where(x => x.Productname.ToLower().Contains(searchText)).ToList();
        }

        ProductsBox.ItemsSource = allProducts;
    }

    private void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addedit = new AddEditProduct(_currentUserId);
        addedit.Show();
        this.Close();
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var main = new MainWindow();
        main.Show();
        this.Close();
    }

    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        Get();
    }

    private void Sort_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();
    }

    private void Filter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }

    private void ProductsBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(ProductsBox.SelectedItem is Product product)
        {
            var addedit = new AddEditProduct(_currentUserId, product);
            addedit.Show();
            this.Close();
        }
    }
}