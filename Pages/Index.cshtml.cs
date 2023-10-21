using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoomWebApp.Controllers;
using SkiaSharp;

namespace RoomWebApp.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public double Height { get; set; }
        [BindProperty]
        public double Width { get; set; }
        [BindProperty]
        public double Length { get; set; }

        public bool ShowResults { get; set; } = false;
        public double Area { get; set; }
        public double PaintNeeded { get; set; }
        public double Volume { get; set; }
        public string ImageData { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            double height = Height;
            double width = Width;
            double length = Length;

            Room r = new Room(height, width, length);

            Area = r.calculateFloorArea();
            PaintNeeded = r.calculatePaintNeeded();
            Volume = r.calculateVolume();   
            generateRoomDiagram();

            ShowResults = true;
            return Page();
        }
        public void generateRoomDiagram()
        {
            double height = Height;
            double width = Width;
            double length = Length;

            // Set the bitmap dimensions appropriately based on room dimensions
            using var bitmap = new SKBitmap(600, 600);

            // Create a canvas from the bitmap
            using var canvas = new SKCanvas(bitmap);

            // Define the positions and dimensions for the isometric projection
            float startX = 200;
            float startY = 150;
            float wallLength = 200;
            float wallHeight = 100;

            // Define the corners of the cuboid
            SKPoint bottomFrontLeft = new SKPoint(startX, startY);
            SKPoint bottomFrontRight = new SKPoint(startX + wallLength, startY);
            SKPoint bottomBackLeft = new SKPoint(startX, startY + wallLength);
            SKPoint bottomBackRight = new SKPoint(startX + wallLength, startY + wallLength);
            SKPoint topFrontLeft = new SKPoint(startX + wallHeight, startY - wallHeight);
            SKPoint topFrontRight = new SKPoint(startX + wallHeight + wallLength, startY - wallHeight);
            SKPoint topBackLeft = new SKPoint(startX + wallHeight, startY - wallHeight + wallLength);
            SKPoint topBackRight = new SKPoint(startX + wallHeight + wallLength, startY - wallHeight + wallLength);

            // Draw the edges of the cuboid
            var paint = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2
            };

            canvas.DrawLine(bottomFrontLeft, bottomFrontRight, paint);
            canvas.DrawLine(bottomFrontRight, bottomBackRight, paint);
            canvas.DrawLine(bottomBackRight, bottomBackLeft, paint);
            canvas.DrawLine(bottomBackLeft, bottomFrontLeft, paint);

            canvas.DrawLine(topFrontLeft, topFrontRight, paint);
            canvas.DrawLine(topFrontRight, topBackRight, paint);
            canvas.DrawLine(topBackRight, topBackLeft, paint);
            canvas.DrawLine(topBackLeft, topFrontLeft, paint);

            canvas.DrawLine(bottomFrontLeft, topFrontLeft, paint);
            canvas.DrawLine(bottomFrontRight, topFrontRight, paint);
            canvas.DrawLine(bottomBackLeft, topBackLeft, paint);
            canvas.DrawLine(bottomBackRight, topBackRight, paint);

            // Add labels for the edges
            paint.TextSize = 18;
            canvas.DrawText(Width.ToString(), startX + wallLength / 2 - 10, startY + wallLength + 30, paint);
            canvas.DrawText(Height.ToString(), startX - 50, startY + wallLength / 2, paint);
            canvas.DrawText(Length.ToString(), startX + wallHeight + wallLength - 70, startY - wallHeight / 2 + wallLength, paint);
            // Calculate and draw the floor area label
            var floorArea = length * width;
            paint.TextSize = 14;
            canvas.DrawText($"Floor Area: {floorArea} sq units", startX + wallLength / 2 - 50, startY + wallLength / 2 + 30, paint);
            // Convert the bitmap to a base64 string for display
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            var bytes = data.ToArray();
            var base64 = Convert.ToBase64String(bytes);
            ImageData = $"data:image/png;base64,{base64}";


        }
    }
}