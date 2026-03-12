using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace WordWrapBug;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        GenerateTests();
        
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Yield();
        await DisplayAlertAsync("Expected Behavior", "Text should not be clipped in any of the 96 boxes.", "Ok");
    }

    void GenerateTests()
    {
        var flows = new[]
        {
            FlowDirection.LeftToRight,
            FlowDirection.RightToLeft
        };

        var textAlignments = new[]
        {
            TextAlignment.Start,
            TextAlignment.Center,
            TextAlignment.End
        };

        LayoutOptions?[] horizontalOptions =
        {
            null,
            LayoutOptions.Start,
            LayoutOptions.Center,
            LayoutOptions.End
        };

        LayoutOptions?[] verticalOptions =
        {
            null,
            LayoutOptions.Start,
            LayoutOptions.Center,
            LayoutOptions.End
        };

        int columnCount = 2;
        int index = 0;

        for (int i = 0; i < columnCount; i++)
            TestGrid.ColumnDefinitions.Add(new ColumnDefinition());

        foreach (var flow in flows)
            foreach (var textAlign in textAlignments)
                foreach (var hOpt in horizontalOptions)
                    foreach (var vOpt in verticalOptions)
                    {
                        int row = index / columnCount;
                        int col = index % columnCount;

                        if (TestGrid.RowDefinitions.Count <= row)
                            TestGrid.RowDefinitions.Add(new RowDefinition { Height = 120 });

                        var label = new Label
                        {
                            Text = "Hello, World!",
                            FontSize = 32,
                            BackgroundColor = Colors.LightGray,
                            WidthRequest = 150,
                            HeightRequest = 100,
                            LineBreakMode = LineBreakMode.WordWrap,
                            FlowDirection = flow,
                            HorizontalTextAlignment = textAlign
                        };

                        if (hOpt != null)
                            label.HorizontalOptions = hOpt.Value;

                        if (vOpt != null)
                            label.VerticalOptions = vOpt.Value;

                        label.AutomationId =
                            $"{flow}_{textAlign}_H{hOpt?.Alignment.ToString() ?? "None"}_V{vOpt?.Alignment.ToString() ?? "None"}";

                        Grid.SetRow(label, row);
                        Grid.SetColumn(label, col);

                        TestGrid.Children.Add(label);

                        index++;
                    }
    }
}