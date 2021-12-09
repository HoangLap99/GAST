using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using TeamplateHotel.Handler;
using TeamplateHotel.Models;

namespace TeamplateHotel.Controllers
{
    public class CommentController : BasicController
    {

        //danh sach ngon ngu
        public static List<Language> GetLanguages()
        {
            using (var db = new MyDbDataContext())
            {
                List<Language> languages = db.Languages.ToList();
                return languages;
            }
        }
        //Chi tiết khách sạn
        public static Hotel DetailHotel(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                var hotel =
                    db.Hotels.FirstOrDefault(a => a.LanguageID == languageKey) ??
                    new Hotel();
                return hotel;
            }
        }
        //danh sách tag aritcle
        public static List<ArticleTag> GetArticleTags(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Article article = db.Articles.FirstOrDefault(a => a.ID == id);
                int[] tagID = article.TagID.Substring(0, article.TagID.Length - 1).Split(',').Select(a => Convert.ToInt32(a)).ToArray();
                List<ArticleTag> tags = new List<ArticleTag>();
                foreach (var item in tagID)
                {
                    tags.Add(db.ArticleTags.FirstOrDefault(a => a.ID == item));
                }
                return tags;
            }
        }
        //Danh sách Main menu
        public static List<Menu> GetMainMenus(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> menus = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.MainMenu &&
                                                       a.LanguageID == languageKey).OrderBy(a => a.Index).ToList();
                return menus;
            }
        }
        //Danh sách Second menu
        public static List<Menu> GetSecondMenus(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<Menu> menufooter = db.Menus.Where(a => a.Status && a.Location == SystemMenuLocation.SecondMenu &&
                                                       a.LanguageID == languageKey).ToList();
                return menufooter;
            }
        }
        //Danh sách Second menu
        public static List<Gallery> Gallery()
        {
            using (var db = new MyDbDataContext())
            {
                List<Gallery> galleries = db.Galleries.OrderByDescending(a => a.ID).ToList();

                return galleries;
            }
        }
        public static List<RoomGallery> RoomGallery()
        {
            using (var db = new MyDbDataContext())
            {
                List<RoomGallery> roomGalleries = db.RoomGalleries.ToList();
                return roomGalleries;
            }
        }
        //get plugin
        public static Plugin GetPluigPlugin()
        {
            using (var db = new MyDbDataContext())
            {
                return db.Plugins.FirstOrDefault() ?? new Plugin();
            }
        }
        //Danh sach slider
        public static List<Slider> GetListSlider(string languageKey, int menuId = 0)
        {
            using (var db = new MyDbDataContext())
            {
                List<Slider> sliders = db.Sliders.Where(a => a.LanguageID == languageKey && a.Status).ToList();
                List<Slider> sliderIsChoise = new List<Slider>();

                //lấy banner theo menu
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                if (menu != null)
                {
                    foreach (var slider in sliders)
                    {
                        if (slider.MenuIDs.Length > 0)
                        {
                            int[] menuIds =
                                slider.MenuIDs.Substring(0, slider.MenuIDs.Length - 1).Split(',').Select(
                                    n => Convert.ToInt32(n)).ToArray();

                            if (menuIds.Contains(menu.ID))
                            {
                                sliderIsChoise.Add(slider);
                            }
                        }
                    }
                }
                else
                {
                    //lấy menu theo trang chủ
                    var menuHome = db.Menus.FirstOrDefault(a => a.Type == SystemMenuType.Home);
                    if (menuHome != null)
                    {
                        foreach (var slider in sliders)
                        {
                            if (slider.MenuIDs.Length > 0)
                            {
                                int[] menuIds =
                           slider.MenuIDs.Substring(0, slider.MenuIDs.Length - 1).Split(',').Select(
                               n => Convert.ToInt32(n)).ToArray();

                                if (menuIds.Contains(menuHome.ID))
                                {
                                    sliderIsChoise.Add(slider);
                                }
                            }
                        }
                    }
                }
                if (sliderIsChoise.Count == 0)
                {
                    sliderIsChoise = sliders;
                }
                return sliderIsChoise;
                //lấy menu hiển thị tất cả
            }
        }


        //Danh sach quang cao
        public static List<Advertising> GetAdvertisings()
        {
            using (var db = new MyDbDataContext())
            {
                List<Advertising> advertisings = db.Advertisings.Where(a => a.Status).ToList();
                return advertisings;
            }
        }

        //Danh sách bài viết theo chuyên mục
        public static IPagedList<Article> GetArticles(int menuId, int? page)
        {
            using (var db = new MyDbDataContext())
            {
                List<Article> articles = db.Articles.Where(a => a.MenuID == menuId && a.Status && a.Home==false).OrderBy(a => a.Createdate).ToList();
                foreach (var article in articles)
                {
                    article.MenuAlias = article.Menu.Alias;
                }
                int pageNumber = (page ?? 1);
                int pageSize = 1;
                return articles.ToPagedList(pageNumber, pageSize);
            }
        }
        public static List<Article> Articles(int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                List<Article> articles = db.Articles.Where(a => a.MenuID == menuId && a.Status).OrderBy(a => a.Index).ToList();
                foreach (var article in articles)
                {
                    article.MenuAlias = article.Menu.Alias;
                }
               
                return articles;
            }
        }
        //Bai viet mới
        public static List<ShowObject> NewArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> articleHots = db.Articles.Where(a => a.New && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description,
                                createdate=a.Createdate.ToString(),
                                //Content = a.Content
                            }).OrderByDescending(a => a.ID).Take(3).ToList();
                return articleHots;
            }
        }
        //Chi tiết bài viết
        public static DetailArticle Detail_Article(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Article article = db.Articles.FirstOrDefault(a => a.ID == id && a.Status) ?? new Article();
                List<Article> articles = db.Articles.Where(a => a.MenuID == article.MenuID && a.Status &&a.Home==false && a.ID != article.ID).OrderBy(a => a.Createdate).ToList();
                foreach (var item in articles)
                {
                    item.MenuAlias = article.Menu.Alias;
                }
                DetailArticle detailArticle = new DetailArticle()
                {
                    Article = article,
                    Articles = articles
                };
                return detailArticle;
            }
        }
        //Danh sách phòng
        public static List<Room> GetRooms(int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.LanguageID == menu.LanguageID).OrderBy(a => a.Index).ToList();

                foreach (var room in rooms)
                {
                    room.MenuAlias = menu.Alias;
                    room.roomGalleries = db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList();
                    room.Price = room.Price * GetPriceUSD.USDToVND();
                }
                return rooms;
            }
        }
        //Chi tiết phòng
        public static DetailRoom Detail_Room(int id, int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                var menu = db.Menus.FirstOrDefault(a => a.ID == menuId);
                Room room = db.Rooms.FirstOrDefault(a => a.ID == id && a.Status) ?? new Room();
                List<RoomGallery> roomGalleries = db.RoomGalleries.Where(a => a.RoomId == room.ID).ToList();
                List<Room> rooms = db.Rooms.Where(a => a.Status && a.ID != room.ID && a.LanguageID == menu.LanguageID).OrderBy(a => a.Index).ToList();

                foreach (var item in rooms)
                {
                    item.MenuAlias = menu.Alias;
                    item.Price = item.Price * GetPriceUSD.USDToVND();
                }
                DetailRoom detailRoom = new DetailRoom()
                {
                    Room = room,
                    RoomGalleries = roomGalleries,
                    Rooms = rooms
                };
                return detailRoom;
            }
        }
        //danh sách employee
        public static List<Employee> employees(string language)
        {
            using (var db=new MyDbDataContext())
            {
                return db.Employees.Where(a => a.LanguageID == language).ToList();
            }    
        }
        //List service
        public static List<ShowObject> ListServiceHome(string language)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> service = db.Services.Where(a => a.Home & a.Status)
                         .Join(db.Menus.Where(a => a.LanguageID == language), a => a.MenuID, b => b.ID,
                             (a, b) => new ShowObject
                             {
                                 ID = a.ID,
                                 Alias = a.Alias,
                                 MenuAlias = b.Alias,
                                 Title = a.Title,
                                 Index = a.Index,
                                 Image = a.Image,
                                 Description = a.Description,
                                 //Content = a.Content
                             }).OrderByDescending(a => a.ID).Take(3).ToList();
                return service;
            }
        }
        
        //Danh sách dich vu
        public static List<Service> GetServices(int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                List<Service> restaurants = db.Services.Where(a => a.Status && a.MenuID == menuId).OrderBy(a => a.Index).ToList();
                foreach (var restaurant in restaurants)
                {
                    restaurant.MenuAlias = restaurant.Menu.Alias;
                }
                return restaurants;
            }
        }

        //Chi tiết dich vu
        public static DetailService Detail_Service(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Service service = db.Services.FirstOrDefault(a => a.ID == id && a.Status) ?? new Service();
                List<ServiceGallery> restaurantGalleries = db.ServiceGalleries.Where(a => a.ServiceID == service.ID).ToList();
                List<Service> restaurants = db.Services.Where(a => a.Status && a.ID != service.ID).OrderBy(a => a.Index).ToList();
                foreach (var item in restaurants)
                {
                    item.MenuAlias = service.Menu.Alias;
                }
                DetailService detailRestaurant = new DetailService()
                {
                    Service = service,
                    ServiceGalleries = restaurantGalleries,
                    Services = restaurants
                };
                return detailRestaurant;
            }
        }

        //Danh sách tours
        public static List<Tour> GetTours(int menuId)
        {
            using (var db = new MyDbDataContext())
            {
                List<Tour> tours = db.Tours.Where(a => a.Status && a.MenuID == menuId).OrderBy(a => a.Index).ToList();
                foreach (var tour in tours)
                {
                    tour.MenuAlias = tour.Menu.Alias;
                }
                return tours;
            }
        }
        //Chi tiết tour
        public static DetailTour Detail_Tour(int id)
        {
            using (var db = new MyDbDataContext())
            {
                Tour tour = db.Tours.FirstOrDefault(a => a.ID == id && a.Status) ?? new Tour();
                List<TourGallery> tourGalleries = db.TourGalleries.Where(a => a.TourID == tour.ID).ToList();
                List<TabTour> tabTours = db.TabTours.Where(a => a.TourID == tour.ID).ToList();
                List<Tour> tours = db.Tours.Where(a => a.Status && a.ID != tour.ID).OrderBy(a => a.Index).ToList();
                foreach (var item in tours)
                {
                    item.MenuAlias = item.Menu.Alias;
                }
                DetailTour detailTour = new DetailTour()
                {
                    Tour = tour,
                    TourGalleries = tourGalleries,
                    Tours = tours,
                    TabTours = tabTours
                };
                return detailTour;
            }
        }

        ///////////// Trang home ////////////////////////
        //Bai viet welcome
        public static ShowObject WellcomeArticle(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                return
                    db.Articles.Where(a => a.Home && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject()
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description
                            }).FirstOrDefault();
            }
        }
        //Bai viet hot
        public static List<ShowObject> HotArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> articleHots = db.Articles.Where(a => a.Hot && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description,
                            }).Take(3).ToList();
                return articleHots;
            }
        }
        //Bai viet Customer
        public static List<ShowObject> CustomerArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> CustomerArticle = db.Articles.Where(a => a.Customer && a.Status)
                        .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                            (a, b) => new ShowObject
                            {
                                ID = a.ID,
                                Alias = a.Alias,
                                MenuAlias = b.Alias,
                                Title = a.Title,
                                Index = a.Index,
                                Image = a.Image,
                                Description = a.Description,
                            }).ToList();
                return CustomerArticle;
            }
        }
        //phòng show home
        public static List<ShowObject> RoomShowHome(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                var memu =
                    db.Menus.FirstOrDefault(a => a.Type == SystemMenuType.RoomRate && a.LanguageID == languageKey) ??
                    new Menu();
                List<ShowObject> roomShowHome = db.Rooms.Where(a => a.Home && a.Status && a.LanguageID == languageKey).Select(a => new ShowObject
                {
                    ID = a.ID,
                    Alias = a.Alias,
                    MenuAlias = memu.Alias,
                    Title = a.Title,
                    Index = a.Index,
                    Image = a.Image,
                    Description = a.Description,
                    Price = (double)a.Price,
                }).ToList();
                foreach(var item in roomShowHome)
                {
                    item.Price = item.Price * (double)GetPriceUSD.USDToVND();
                }
                return roomShowHome;
            }
        }
        public static List<ShowObject> tourArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> articleHots = db.Tours.Where(a => a.ShowHome && a.Status)
                .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                (a, b) => new ShowObject
                {
                    ID = a.ID,
                    Alias = a.Alias,
                    MenuAlias = b.Alias,
                    Title = a.Title,
                    Index = a.Index,
                    Image = a.Image,
                    Description = a.Description,
                }).ToList();
                return articleHots;
            }
        }
        //Bai viet service
        public static List<ShowObject> serviceArticles(string languageKey)
        {
            using (var db = new MyDbDataContext())
            {
                List<ShowObject> articleHots = db.Services.Where(a => a.Home && a.Status)
                .Join(db.Menus.Where(a => a.LanguageID == languageKey), a => a.MenuID, b => b.ID,
                (a, b) => new ShowObject
                {
                    ID = a.ID,
                    Alias = a.Alias,
                    MenuAlias = b.Alias,
                    Title = a.Title,
                    Index = a.Index,
                    Image = a.Image,
                    Description = a.Description,
                }).ToList();
                return articleHots;
            }
        }

    }
}