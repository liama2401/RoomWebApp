using Microsoft.AspNetCore.Mvc;

namespace RoomWebApp.Controllers
{
    public class RoomController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calculate(double height, double width, double length)
        {
            var r = new Room(height, width, length);
            ViewBag.Area = r.calculateFloorArea();
            ViewBag.PaintNeeded = r.calculatePaintNeeded();
            ViewBag.Volume = r.calculateVolume();
            return View("Result");
        }
    }
    //Assumptions made are that the room is cuboid and that it takes 6.5 meters squared is painted by 1 litre of paint
    public class Room
    {
        private double height, width, length;

        public Room(double height, double width, double length)
        {
            this.height = height;
            this.width = width;
            this.length = length;
        }

        public double calculateFloorArea()
        {
            return width * length;
        }

        public double calculatePaintNeeded()
        {
            return (width + length) * (height * 2 / 6.5);
        }

        public double calculateVolume()
        {
            return height * width * length;
        }
    }

}
