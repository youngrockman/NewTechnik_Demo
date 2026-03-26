using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using System;
using System.IO;
using System.Linq;
using test_demo.Models;

namespace test_demo;

public partial class AddEditProduct : Window
{
    private Product _product;
    private int _currentUser;
    private string ImageName = Guid.NewGuid().ToString("N");

    public AddEditProduct()
    {
        InitializeComponent();
    }
    public AddEditProduct(int currentUserId)
    {
        InitializeComponent();
        _currentUser = currentUserId;
        LoadManu();
        LoadSup();
        LoadCat();
        DataContext = new Product();
    }

    public AddEditProduct(int currentUserId, Product product)
    {
        InitializeComponent();
        using var context = new DemoContext();
        _currentUser = currentUserId;
        _product = product;
        ImageName = _product.Productphoto ?? Guid.NewGuid().ToString("N");
        LoadManu();
        LoadSup();
        LoadCat();
        DataContext = _product;
        EditBut.IsVisible = true;
        DeleteBut.IsVisible = true;

        try
        {
            if (!string.IsNullOrEmpty(_product.Productphoto))
            {
                ImageBox.Source = new Bitmap(_product.Productphoto);
            }
            else
            {
                ImageBox.Source = new Bitmap("picture.png");
            }
        }
        catch (Exception ex)
        {
           
        }




        var a = _product.Productmanufacturer;
        var b = _product.Productcategory;
        var c = _product.Supplierid;

        Supplier.SelectedItem = context.Suppliers.Where(x => x.Supplierid == c).Select(x => x.Suppliername).FirstOrDefault();
        Manufacturer.SelectedItem = context.Manufacturers.Where(x => x.Manufacturerid == a).Select(x =>x.Manufacturername).FirstOrDefault();
        Category.SelectedItem = context.Productcategories.Where(x => x.Productcategoryid == b).Select(x => x.Productcategoryname).FirstOrDefault();
    }   

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var catalog = new CatalogWindow(_currentUser);
        catalog.Show();
        this.Close();
    }

    private async void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            using var context = new DemoContext();
            var newProduct = DataContext as Product;

            if (Manufacturer.SelectedItem != null && Supplier.SelectedItem != null && Category.SelectedItem != null)
            {
                var man = Manufacturer.SelectedItem.ToString();
                var sup = Supplier.SelectedItem.ToString();
                var cat = Category.SelectedItem.ToString();


                var manFin = context.Manufacturers.Where(x => x.Manufacturername == man).Select(x => x.Manufacturerid).FirstOrDefault();
                var supFin = context.Suppliers.Where(x => x.Suppliername == sup).Select(x => x.Supplierid).FirstOrDefault();
                var catFin = context.Productcategories.Where(x => x.Productcategoryname == cat).Select(x => x.Productcategoryid).FirstOrDefault();

                newProduct.Supplierid = supFin;
                newProduct.Productmanufacturer = manFin;
                newProduct.Productcategory = catFin;
                newProduct.Productphoto = ImageName;

                context.Products.Add(newProduct);
                await context.SaveChangesAsync();

                var nice = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар создан", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await nice.ShowAsync();

                var catalog = new CatalogWindow(_currentUser);
                catalog.Show();
                this.Close();
            }
        }
        catch (Exception ex)
        {
            var excep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", excep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            error.ShowAsync();

        }
    }

    private void LoadManu()
    {
        using var context = new DemoContext();
        var man = context.Manufacturers.Select(x => x.Manufacturername).ToList();
        Manufacturer.ItemsSource = man;
    }
    private void LoadSup()
    {
        using var context = new DemoContext();
        var sup = context.Suppliers.Select(x => x.Suppliername).ToList();
        Supplier.ItemsSource = sup;

    }
    private void LoadCat()
    {
        using var context = new DemoContext();
        var cat = context.Productcategories.Select(x => x.Productcategoryname).ToList();
        Category.ItemsSource = cat;

    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
            {
                Title = "Добавить кратинку",
                FileTypeChoices = new[]
                {
                FilePickerFileTypes.All
            }
            });

            if (file != null)
            {
                ImageBox.Source = new Bitmap(file.Path.LocalPath);
                var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImageName + Path.GetExtension(file.Name));
                File.Copy(file.Path.LocalPath, targetPath);
                ImageName = targetPath;

            }

        }
        catch { }
    }

    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DemoContext();

        var productId = _product.Productarticlenumber;

        var productToDelete = context.Products.Where(x=>x.Productarticlenumber == productId).FirstOrDefault();

        if(productToDelete != null)
        {
            context.Remove(productToDelete);
            context.SaveChanges();
        }

        var nice = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар удален", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await nice.ShowAsync();

        var catalog = new CatalogWindow(_currentUser);
        catalog.Show();
        this.Close();
    }

    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DemoContext();

        try
        {

                var man = Manufacturer.SelectedItem.ToString();
                var sup = Supplier.SelectedItem.ToString();
                var cat = Category.SelectedItem.ToString();


                var manFin = context.Manufacturers.Where(x => x.Manufacturername == man).Select(x => x.Manufacturerid).FirstOrDefault();
                var supFin = context.Suppliers.Where(x => x.Suppliername == sup).Select(x => x.Supplierid).FirstOrDefault();
                var catFin = context.Productcategories.Where(x => x.Productcategoryname == cat).Select(x => x.Productcategoryid).FirstOrDefault();



                _product.Supplierid = supFin;
                _product.Productmanufacturer = manFin;
                _product.Productcategory = catFin;
                _product.Productphoto = ImageName;

                context.Products.Update(_product);
                await context.SaveChangesAsync();

                var nice = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар изменен", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await nice.ShowAsync();

                var catalog = new CatalogWindow(_currentUser);
                catalog.Show();
                this.Close();
            
        }
        catch (Exception ex)
        { 
            var exec = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", exec, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            error.ShowAsync();

        }
    }
}