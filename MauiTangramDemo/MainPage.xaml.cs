using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace MauiTangramDemo;

public partial class MainPage : ContentPage
{
    SKBitmap? tangramzones;
    static Dictionary<Color, string> tangramdict = new Dictionary<Color, string>
    {
        { Color.FromArgb("#FF7476"), "Large Left Triangle" },
        { Color.FromArgb("#DA9AFF"), "Large Top Triangle" },
        { Color.FromArgb("#8994FF"), "Small Middle Triangle" },
        { Color.FromArgb("#2DD998"), "Small Bottom Triangle" },
        { Color.FromArgb("#FF8902"), "Middle Triangle" },
        { Color.FromArgb("#65FFCC"), "Square" },
        { Color.FromArgb("#FFCD1F"), "Parallelogram" },
    };

    SKBitmap? manzones;
    static Dictionary<Color, string> mandict = new Dictionary<Color, string>
    {
        { Color.FromArgb("#65FFCC"), "Head" },
        { Color.FromArgb("#DA9AFF"), "Chest" },
        { Color.FromArgb("#FF7476"), "Stomach" },
        { Color.FromArgb("#FFCD1F"), "Top leg" },
        { Color.FromArgb("#2DD998"), "Top foot" },
        { Color.FromArgb("#FF8902"), "Bottom leg" },
        { Color.FromArgb("#8994FF"), "Bottom foot" },
    };

    public MainPage()
    {
        InitializeComponent();
        Dispatcher.Dispatch(async () =>
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("tangram_zones.png");
            tangramzones = SKBitmap.Decode(stream);
            var manstream = await FileSystem.OpenAppPackageFileAsync("tangram_man_zones.png");
            manzones = SKBitmap.Decode(manstream);
        });
    }

    /// <summary>
    /// Demonstrates detection of a user click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IdentifyTangramPieces(object sender, TappedEventArgs e)
    {
        if (tangramzones is null) return;

        // Get the position of the tap relative to the view
        View view = (View)sender;
        Point? pos = e.GetPosition(view);
        if (pos is null) return;

        // Determine the pixel color of the tap
        int normalizedX = (int)(pos.Value.X * tangramzones.Width / view.Width);
        int normalizedY = (int)(pos.Value.Y * tangramzones.Height / view.Height);
        SKColor skcolor = tangramzones.GetPixel(normalizedX, normalizedY);
        Color color = skcolor.ToMauiColor();

        // Determine the item at the tap location
        if (tangramdict.ContainsKey(color))
        {
            string item = tangramdict[color];
            tangramInfo.Text = $"{item} found at {normalizedX},{normalizedY}";
        }
        else
        {
            tangramInfo.Text = $"No item found at {normalizedX},{normalizedY} with color {color.ToHex()}";
        }
    }

    /// <summary>
    /// Demonstrates detection of a user click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void IdentifyMan(object sender, TappedEventArgs e)
    {
        if (manzones is null) return;

        // Get the position of the tap relative to the view
        View view = (View)sender;
        Point? pos = e.GetPosition(view);
        if (pos is null) return;

        // Determine the pixel color of the tap
        int normalizedX = (int)(pos.Value.X * manzones.Width / view.Width);
        int normalizedY = (int)(pos.Value.Y * manzones.Height / view.Height);
        SKColor skcolor = manzones.GetPixel(normalizedX, normalizedY);
        Color color = skcolor.ToMauiColor();

        // Determine the item at the tap location
        if (mandict.ContainsKey(color))
        {
            string item = mandict[color];
            manInfo.Text = $"{item} found at {normalizedX},{normalizedY}";
        }
        else
        {
            manInfo.Text = $"No item found at {normalizedX},{normalizedY} with color {color.ToHex()}";
        }
    }
}
