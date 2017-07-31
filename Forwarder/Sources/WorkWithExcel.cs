using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Forwarder.Sources
{
    class WorkWithExcel
    {
        public static void ForwardersOnBusinessTrips (List<ClassResource.Forwarder> forwarders, List<ClassResource.Route> routes, List<ClassResource.Destination> destinations, List<ClassResource.Company> companies, List<ClassResource.Request> requests)
        {
            Excel.Application excelapp = new Excel.Application()
            {
                Visible = true
            };
            Excel.Range excelcells;
            excelapp.SheetsInNewWorkbook = 1;
            excelapp.Workbooks.Add(Type.Missing);

            Excel.Sheets excelsheets = excelapp.Worksheets;
            Excel.Worksheet excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);

            excelcells = excelworksheet.get_Range("A1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Экспедитор";
            excelcells = excelworksheet.get_Range("B1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Дата убытия";
            excelcells = excelworksheet.get_Range("C1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Пункт назначения";
            excelcells = excelworksheet.get_Range("D1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Товар";

            int rowCount = 2;

            foreach (var forwarder in forwarders.FindAll(x => routes.Find(y => y.RouteStatus == "Закрыт" && y.IDForwarder == x.ID) != null))
            {
                excelcells = excelworksheet.get_Range("A" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = forwarder.Name;
                excelcells = excelworksheet.get_Range("B" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = routes.Find(y => y.RouteStatus == "Закрыт" && y.IDForwarder == forwarder.ID).DepartureDate;

                List<ClassResource.Destination> temp = destinations.FindAll(x => x.IDRoute == routes.Find(y => y.RouteStatus == "Закрыт" && y.IDForwarder == forwarder.ID).ID);
                temp.Sort((a, b) => (a.Number.CompareTo(b.Number)));

                for (int i = 0; i < temp.Count; i++)
                {
                    if (i != 0)
                    {
                        if (companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i - 1].IDRequest).IDCompany).City != companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).City)
                        {
                            excelcells = excelworksheet.get_Range("C" + rowCount.ToString(), Type.Missing);
                            excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).City;
                        }
                    }
                    else
                    {
                        excelcells = excelworksheet.get_Range("C" + rowCount.ToString(), Type.Missing);
                        excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).City;
                    }
                    excelcells = excelworksheet.get_Range("D" + rowCount.ToString(), Type.Missing);
                    excelcells.Value2 = requests.Find(y => y.ID == temp[i].IDRequest).ProductName;

                    rowCount++;
                }
            }

            excelcells = (Excel.Range)excelworksheet.Columns["A:A", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["B:B", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["C:C", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["D:D", Type.Missing];
            excelcells.Columns.AutoFit();
        }

        public static void StatusRequest(List<ClassResource.Route> routes, List<ClassResource.Destination> destinations, List<ClassResource.Request> requests)
        {
            Excel.Application excelapp = new Excel.Application()
            {
                Visible = true
            };
            Excel.Range excelcells;
            excelapp.SheetsInNewWorkbook = 1;
            excelapp.Workbooks.Add(Type.Missing);

            Excel.Sheets excelsheets = excelapp.Worksheets;
            Excel.Worksheet excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);

            excelcells = excelworksheet.get_Range("A1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Номер заявки";
            excelcells = excelworksheet.get_Range("B1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Наименование товара";
            excelcells = excelworksheet.get_Range("C1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Статус заявки";

            int rowCount = 2;

            foreach (var request in requests)
            {
                excelcells = excelworksheet.get_Range("A" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = request.Number;
                excelcells = excelworksheet.get_Range("B" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = request.ProductName;
                excelcells = excelworksheet.get_Range("C" + rowCount.ToString(), Type.Missing);
                if (destinations.Find(x => x.IDRequest == request.ID) != null)
                {
                    if (routes.Find(x => x.ID == destinations.Find(y => y.IDRequest == request.ID).IDRoute).RouteStatus == "Открыт")
                    {
                        excelcells.Value2 = "Закреплена";
                    }
                    else if (routes.Find(x => x.ID == destinations.Find(y => y.IDRequest == request.ID).IDRoute).RouteStatus == "Закрыт")
                    {
                        excelcells.Value2 = "Выполняется";
                    }
                    else if (routes.Find(x => x.ID == destinations.Find(y => y.IDRequest == request.ID).IDRoute).RouteStatus == "Завершен")
                    {
                        excelcells.Value2 = "Выполнена";
                    }
                }
                else
                {
                    excelcells.Value2 = "В обработке";
                }
            }

            excelcells = (Excel.Range)excelworksheet.Columns["A:A", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["B:B", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["C:C", Type.Missing];
            excelcells.Columns.AutoFit();
        }

        public static void ReportDestination(List<ClassResource.Destination> destinations, List<ClassResource.Request> requests, List<ClassResource.Company> companies)
        {
            Excel.Application excelapp = new Excel.Application()
            {
                Visible = true
            };
            Excel.Range excelcells;
            excelapp.SheetsInNewWorkbook = 1;
            excelapp.Workbooks.Add(Type.Missing);

            Excel.Sheets excelsheets = excelapp.Worksheets;
            Excel.Worksheet excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);

            excelcells = excelworksheet.get_Range("A1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "№";
            excelcells = excelworksheet.get_Range("B1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Наименование товара";
            excelcells = excelworksheet.get_Range("C1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Количество";
            excelcells = excelworksheet.get_Range("D1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Фирма";
            excelcells = excelworksheet.get_Range("E1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Контактное лицо";
            excelcells = excelworksheet.get_Range("F1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Телефон";
            excelcells = excelworksheet.get_Range("G1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Точный адрес";

            int rowCount = 2;

            foreach (var destination in destinations)
            {
                excelcells = excelworksheet.get_Range("A" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = destination.Number;
                excelcells = excelworksheet.get_Range("B" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = requests.Find(x => x.ID == destination.IDRequest).ProductName;
                excelcells = excelworksheet.get_Range("C" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = requests.Find(x => x.ID == destination.IDRequest).Quantity;
                excelcells = excelworksheet.get_Range("D" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = companies.Find(y => y.ID == requests.Find(x => x.ID == destination.IDRequest).IDCompany).Name;
                excelcells = excelworksheet.get_Range("E" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = companies.Find(y => y.ID == requests.Find(x => x.ID == destination.IDRequest).IDCompany).NameСontactPerson;
                excelcells = excelworksheet.get_Range("F" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = companies.Find(y => y.ID == requests.Find(x => x.ID == destination.IDRequest).IDCompany).PhoneContactPerson;
                excelcells = excelworksheet.get_Range("G" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = companies.Find(y => y.ID == requests.Find(x => x.ID == destination.IDRequest).IDCompany).Address;

                rowCount++;
            }

            excelcells = (Excel.Range)excelworksheet.Columns["A:A", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["B:B", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["C:C", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["D:D", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["E:E", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["F:F", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["G:G", Type.Missing];
            excelcells.Columns.AutoFit();
        }

        public static void TotalReport(List<ClassResource.Forwarder> forwarders, List<ClassResource.Route> routes, List<ClassResource.Destination> destinations, List<ClassResource.Company> companies, List<ClassResource.Request> requests)
        {
            Excel.Application excelapp = new Excel.Application()
            {
                Visible = true
            };
            Excel.Range excelcells;
            excelapp.SheetsInNewWorkbook = 1;
            excelapp.Workbooks.Add(Type.Missing);

            Excel.Sheets excelsheets = excelapp.Worksheets;
            Excel.Worksheet excelworksheet = (Excel.Worksheet)excelsheets.get_Item(1);

            excelcells = excelworksheet.get_Range("A1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Экспедитор";
            excelcells = excelworksheet.get_Range("B1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Дата убытия";
            excelcells = excelworksheet.get_Range("C1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Дата возвращения";
            excelcells = excelworksheet.get_Range("D1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Пункт назначения";
            excelcells = excelworksheet.get_Range("E1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Фирма";
            excelcells = excelworksheet.get_Range("F1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Адрес";
            excelcells = excelworksheet.get_Range("G1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Дата прибытия";
            excelcells = excelworksheet.get_Range("H1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Наименование товара";
            excelcells = excelworksheet.get_Range("I1", Type.Missing);
            excelcells.EntireRow.Font.Bold = true;
            excelcells.Value2 = "Количество";
            

            int rowCount = 2;

            foreach (var forwarder in forwarders.FindAll(x => routes.Find(y => y.RouteStatus != "Открыт" && y.IDForwarder == x.ID) != null))
            {
                excelcells = excelworksheet.get_Range("A" + rowCount.ToString(), Type.Missing);
                excelcells.Value2 = forwarder.Name;

                foreach (var route in routes.FindAll(y => y.RouteStatus != "Открыт" && y.IDForwarder == forwarder.ID))
                {
                    excelcells = excelworksheet.get_Range("B" + rowCount.ToString(), Type.Missing);
                    excelcells.Value2 = route.DepartureDate;

                    excelcells = excelworksheet.get_Range("C" + rowCount.ToString(), Type.Missing);
                    excelcells.Value2 = route.ReturnDate;

                    List<ClassResource.Destination> temp = destinations.FindAll(x => x.IDRoute == route.ID);
                    temp.Sort((a, b) => (a.Number.CompareTo(b.Number)));

                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i - 1].IDRequest).IDCompany).City != companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).City)
                            {
                                excelcells = excelworksheet.get_Range("D" + rowCount.ToString(), Type.Missing);
                                excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).City;
                                excelcells = excelworksheet.get_Range("E" + rowCount.ToString(), Type.Missing);
                                excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).Name;
                                excelcells = excelworksheet.get_Range("F" + rowCount.ToString(), Type.Missing);
                                excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).Address;
                            }
                        }
                        else
                        {
                            excelcells = excelworksheet.get_Range("D" + rowCount.ToString(), Type.Missing);
                            excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).City;
                            excelcells = excelworksheet.get_Range("E" + rowCount.ToString(), Type.Missing);
                            excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).Name;
                            excelcells = excelworksheet.get_Range("F" + rowCount.ToString(), Type.Missing);
                            excelcells.Value2 = companies.Find(x => x.ID == requests.Find(y => y.ID == temp[i].IDRequest).IDCompany).Address;
                        }

                        excelcells = excelworksheet.get_Range("G" + rowCount.ToString(), Type.Missing);
                        excelcells.Value2 = temp[i].ArrivalDate;

                        excelcells = excelworksheet.get_Range("H" + rowCount.ToString(), Type.Missing);
                        excelcells.Value2 = requests.Find(y => y.ID == temp[i].IDRequest).ProductName;
                        excelcells = excelworksheet.get_Range("I" + rowCount.ToString(), Type.Missing);
                        excelcells.Value2 = requests.Find(y => y.ID == temp[i].IDRequest).Quantity;

                        rowCount++;
                    }
                }
            }

            excelcells = (Excel.Range)excelworksheet.Columns["A:A", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["B:B", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["C:C", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["D:D", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["E:E", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["F:F", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["G:G", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["H:H", Type.Missing];
            excelcells.Columns.AutoFit();
            excelcells = (Excel.Range)excelworksheet.Columns["I:I", Type.Missing];
            excelcells.Columns.AutoFit();
        }
    }
}
