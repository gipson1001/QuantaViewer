using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Data.OleDb;
using System.Data;

using GUtils;

namespace QuantaViewer {
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form {
		public List<string> fileList;
		public List<Employee> employeeList;
		public Dictionary<string, int> orgCount;
		public const string FILES_PATH = ".\\qdata\\";
		public Dictionary<long, string> fileHash;

		public MainForm () {
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			fileHash = new Dictionary<long, string>();
			this.buComboBox.SelectedIndexChanged += delegate {
				string bu = this.buComboBox.Text;
				
				if(bu.Length > 0) {
					List<Employee> newList = new List<MainForm.Employee>();
					
					foreach(Employee item in employeeList) {
						if(item.org.StartsWith(bu)) {
							newList.Add(item);
						}
					}
					
					analyzeYear(newList);
				} else {
					analyzeYear(employeeList);
				}
			};
			
			this.updateButton.Click += delegate {
				String h1 = "https://dl.dropboxusercontent.com";
				String h2 = "/u/";
				String h3 = "135" + 8 + "874";
				String h4 = "4";
				String h5 = "/quanta/";
				if(GUtils.StaticTool.checkHostConnectable("www.yahoo.com", 80, 2)) {
					String content = GUtils.HTTPTool.fetchHttpContentInQ(
						                 h1 + h2 + h3 + h4 + h5 + "quanta_list.txt");
					if(content != null) {
						List<List<string>> data = Converter.csv2ListArray(content);
						foreach(List<string> item in data) {
							if(item[0].ToLower().EndsWith(".csv")) {
								if(!fileList.Contains(FILES_PATH + item[0])) {
									String csv = HTTPTool.fetchHttpContentInQ(h1 + h2 + h3 + h4 + h5 + item[0]);
									File.WriteAllText(FILES_PATH + item[0], csv);
									addCSVFile(item[0]);
									this.updateButton.Text = "已下載";
								} else {
									this.updateButton.Text = "已最新";
								}
								break;
							}
						}
					} else {
						this.updateButton.Text = "無資料";
					}
				} else {
					this.updateButton.Text = "沒網路";
				}

				/*using (var client = new WebClient())
				{
					client.DownloadFile("http://qciewcdb/files/2007/sa/g.xls", "test.xls");
				}
				
				if(File.Exists("test.xls")) {
					//CovertExcelToCsv("test.xls", "test.csv", 0);
				}*/

				/*Stream stream = GUtils.HTTPTool.fetchHttpStream("http://qciewcdb/files/2007/sa/g.xls");
				FileStream fileStream = File.Create("test.xls", 1024);
				byte[] bytesInStream = new byte[stream.Length];
				stream.Read(bytesInStream, 0, bytesInStream.Length);
				fileStream.Write(bytesInStream, 0, bytesInStream.Length);*/
				this.updateButton.Enabled = false;
			};
			this.parseButton.Click += delegate {
				if(fileListView.SelectedItems.Count == 0) {
					chagneTextBox.Text = "先選一個檔案";
				} else {
					if(parseTextBox.Text.Length > 0) {
						List<Employee> list = parseContent(parseTextBox.Text);
						list.Sort(
							delegate(Employee x,Employee y) {
								return x.org.CompareTo(y.org);
							});
						chagneTextBox.Clear();
						StringBuilder sb = new StringBuilder();
						sb.AppendLine("總數: " + list.Count);
						foreach(Employee item in list) {
							sb.AppendLine(item.ToString());
						}
						
						chagneTextBox.Text = sb.ToString();
					}
				}
			};
			
			this.emailButton.Click += delegate {
				if(fileListView.SelectedItems.Count == 0) {
					chagneTextBox.Text = "先選一個檔案";
				} else {
					if(parseTextBox.Text.Length > 0) {
						List<Employee> list = parseContent(parseTextBox.Text);
						list.Sort(
							delegate(Employee x,Employee y) {
								return x.org.CompareTo(y.org);
							});
						chagneTextBox.Clear();
						StringBuilder sb = new StringBuilder();
						sb.AppendLine("總數: " + list.Count);
						foreach(Employee item in list) {
							sb.AppendLine(item.email + "(" + item.chtName + ")");
						}
						
						chagneTextBox.Text = sb.ToString();
					}
				}
			};
			
			this.filterButton.Click += delegate {
				if(fileListView.SelectedItems.Count == 0) {
					chagneTextBox.Text = "先選一個檔案";
				} else {
					string selectedTitle = titleFilterComboBox.SelectedItem as string;
					List<Employee> list = filterEmployee(selectedTitle, yearFilterTextBox.Text);
					list.Sort(
						delegate(Employee x,Employee y) {
							return x.org.CompareTo(y.org);
						});
					chagneTextBox.Clear();
					StringBuilder sb = new StringBuilder();
					sb.AppendLine("總數: " + list.Count);
					foreach(Employee item in list) {
						sb.AppendLine(item.ToString());
					}

					chagneTextBox.Text = sb.ToString();
				}
			};

			this.clearButton.Click += delegate {
				parseTextBox.Clear();
			};

			fileList = new List<string>();
			this.fileListView.ItemSelectionChanged += selectedIndexChanged;
			
			if(!Directory.Exists(FILES_PATH)) {
				Directory.CreateDirectory(FILES_PATH);
			}
			
			string[] files = Directory.GetFiles(FILES_PATH);
			fileList.AddRange(files);
			if(files.Length > 0) {
				foreach(string item in files) {
					if(item.EndsWith(".csv")) {
						addCSVFile(item.Replace(FILES_PATH, ""));
					}
				}
			}
		}
		
		static void CovertExcelToCsv(string excelFilePath,string csvOutputFile,int worksheetNumber = 1) {
			var cnnStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath + ";Extended Properties=Excel 12.0;");
			var cnn = new OleDbConnection(cnnStr);
		
			var dt = new DataTable();
			try {
				cnn.Open();
				var schemaTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
				if(schemaTable.Rows.Count < worksheetNumber) throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");
				string worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
				string sql = String.Format("select * from [{0}]", worksheet);
				var da = new OleDbDataAdapter(sql,cnn);
				da.Fill(dt);
			} catch(Exception e) {
				throw e;
			} finally {
				cnn.Close();
			}

			using(var wtr = new StreamWriter(csvOutputFile)) {
				foreach(DataRow row in dt.Rows) {
					bool firstLine = true;
					foreach(DataColumn col in dt.Columns) {
						if(firstLine) {
							wtr.Write(",");
						} else {
							firstLine = false;
						}
						var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
						wtr.Write(String.Format("\"{0}\"", data));
					}
					wtr.WriteLine();
				}
			}
		}
		
		
		private void addCSVFile(string file) {
			Match m = Regex.Match(file, @"\d{8}");
			if(m != null) {
				string dateStr = m.Value;
				int year = int.Parse(dateStr.Substring(0, 4));
				int month = int.Parse(dateStr.Substring(4, 2));
				int day = int.Parse(dateStr.Substring(6, 2));
				//int hour = int.Parse(file.Substring(15, 2));
				//int min = int.Parse(file.Substring(17, 2));
				DateTime date = new DateTime(year,month,day);
				if(!fileHash.ContainsKey(date.Ticks)) {
					fileHash.Add(date.Ticks, file);
					fileListView.Items.Add(date.ToShortDateString());
				}
			}
		}
		
		private List<Employee> parseContent(string content) {
			string pattern1 = @"\d{8}";
			string pattern2 = @"\d{6}[A-Z]{1}\d";
			string emailPattern = @"<[^>]*>";
			int count = 0;
			List<string> aliasList = new List<string>();
			List<string> emailList = new List<string>();
			List<string> chtNameList = new List<string>();
			
			foreach(Match m in Regex.Matches(content, pattern1)) {
				count++;
				aliasList.Add(m.Value);
			}
			
			foreach(Match m in Regex.Matches(content, pattern2)) {
				count++;
				aliasList.Add(m.Value);
			}
			
			foreach(Match m in Regex.Matches(content, emailPattern)) {
				count++;
				emailList.Add(m.Value.Replace("<", "").Replace(">", "").ToLower());
			}
			
			chtNameList.AddRange(content.Split(new string[] {
				",",
				"\n",
				" ",
				"#",
				"(",
				")"
			}, StringSplitOptions.RemoveEmptyEntries));
			
			List<Employee> ret = new List<Employee>();
			List<string> savedAlias = new List<string>();
			Dictionary<string, Employee> employeeMap = new Dictionary<string, Employee>();
			Dictionary<string, Employee> emailMap = new Dictionary<string, Employee>();
			Dictionary<string, List<Employee>> chtNameMap = new Dictionary<string, List<Employee>>();
			
			if(employeeList.Count > 0) {
				foreach(Employee item in employeeList) {
					employeeMap.Add(item.alias, item);
				}
				foreach(Employee item in employeeList) {
					emailMap.Add(item.email.ToLower(), item);
				}
				foreach(Employee item in employeeList) {
					if(!chtNameMap.ContainsKey(item.chtName)) {
						chtNameMap.Add(item.chtName, new List<Employee>());
					}
					chtNameMap[item.chtName].Add(item);
				}
				
				foreach(string item in aliasList) {
					if(employeeMap.ContainsKey(item)) {
						ret.Add(employeeMap[item]);
						savedAlias.Add(employeeMap[item].alias);
					}
				}
				
				foreach(string item in emailList) {
					if(emailMap.ContainsKey(item) && !savedAlias.Contains(emailMap[item].alias)) {
						ret.Add(emailMap[item]);
						savedAlias.Add(emailMap[item].alias);
					}
				}
				
				foreach(string item in chtNameList) {
					if(chtNameMap.ContainsKey(item)) {
						foreach(Employee e in chtNameMap[item]) {
							if(!savedAlias.Contains(e.alias)) {
								ret.Add(e);
							}
						}
					}
				}
			}
			
			System.Console.WriteLine(count);
			return ret;
		}

		private List<Employee> filterEmployee(string title,string year) {
			int count = 0;
			List<string> targetList = new List<string>();
			
			List<Employee> ret = new List<MainForm.Employee>();
			Dictionary<string, Employee> employeeMap = new Dictionary<string, MainForm.Employee>();
			
			if(employeeList.Count > 0) {
				foreach(Employee item in employeeList) {
					if((title != null && title.Length > 0 && !item.title.Equals(title))) {
						continue;
					}
					
					if((year != null && year.Length > 0 && !item.alias.StartsWith(year))) {
						continue;
					}

					ret.Add(item);
				}
			}
			
			System.Console.WriteLine(count);
			return ret;
		}
		
		private string getPathFromDate(string dateStr) {
			string[] strs = dateStr.Split(new char[]{ '/' }, StringSplitOptions.RemoveEmptyEntries);
			int year = int.Parse(strs[0]);
			int month = int.Parse(strs[1]);
			int day = int.Parse(strs[2]);
			DateTime date = new DateTime(year,month,day);
			if(fileHash.ContainsKey(date.Ticks)) {
				return FILES_PATH + fileHash[date.Ticks];
			}
			
			return null;
		}
		
		private void selectedIndexChanged(Object sender,EventArgs e) {
			if(fileListView.SelectedItems.Count == 2) {
				List<Employee> data1 = getEmployee(getPathFromDate(fileListView.SelectedItems[0].Text));
				List<Employee> data2 = getEmployee(getPathFromDate(fileListView.SelectedItems[1].Text));
				employeeList = getEmployee(getPathFromDate(fileListView.SelectedItems[0].Text));
				compare(data1, data2);
			} else if(fileListView.SelectedItems.Count == 1) {
				employeeList = getEmployee(getPathFromDate(fileListView.SelectedItems[0].Text));
				detail();
			}
			
			List<string> titleList = new List<string>();
			if(employeeList.Count > 0) {
				foreach(Employee item in employeeList) {
					if(!titleList.Contains(item.title)) {
						titleList.Add(item.title);
					}
				}
				titleList.Sort(
					delegate(string x,string y) {
						if(getWeight(x) == getWeight(y)) {
							return 0;
						} else if(getWeight(x) > getWeight(y)) {
							return -1;
						} else {
							return 1;
						}
					});
				titleFilterComboBox.Items.AddRange(titleList.ToArray());
			}
		}
		
		private string simpleOrg(string org) {
			if(org.Contains("事業部")) {
				org = org.Substring(0, org.IndexOf("事業部") + 3);
			} else if(org.Contains("事業組")) {
				org = org.Substring(0, org.IndexOf("事業組") + 3);
			} else if(org.StartsWith("QCMC")) {
				org = org.Substring(0, org.IndexOf("QCMC") + 4);
			} else if(org.StartsWith("QSMC")) {
				org = org.Substring(0, org.IndexOf("QSMC") + 4);
			} else if(org.Contains("營運群")) {
				org = org.Substring(0, org.IndexOf("營運群") + 3);
			} else if(org.Contains("研究院")) {
				org = org.Substring(0, org.IndexOf("研究院") + 3);
			} else if(org.Contains("中心")) {
				org = org.Substring(0, org.IndexOf("中心") + 2);
			} else if(org.Contains("服務處")) {
				org = org.Substring(0, org.IndexOf("服務處") + 3);
			} else if(org.Contains("管理處")) {
				org = org.Substring(0, org.IndexOf("管理處") + 3);
			} else if(org.Contains("總經理室")) {
				org = org.Substring(0, org.IndexOf("總經理室") + 4);
			} else if(org.Contains("觸控小組")) {
				org = org.Substring(0, org.IndexOf("觸控小組") + 4);
			}
			
			return org;
		}
		
		private string simpleTitle(string title) {
			if(!title.Contains("技術員")) {
				title = title.Replace("專案", "").Replace("技術", "");
			}
			
			return title;
		}
		
		private void detail() {
			Dictionary<string, int> titleCount = new Dictionary<string, int>();
			List<string> orgList = new List<string>();
			orgCount = new Dictionary<string, int>();
			
			foreach(Employee item in employeeList) {
				string title = item.title;
				
				if(title.Contains("顧問")) {
					title = "顧問";
				} else if(title != "技術員") {
					title = title.Replace("技術", "").Replace("專案", "").Replace("研究", "");
				}
				
				if(!titleCount.ContainsKey(title)) {
					titleCount.Add(title, 1);
				} else {
					titleCount[title]++;
				}
				
				string org = simpleOrg(item.org);
				
				if(!orgCount.ContainsKey(org)) {
					orgCount.Add(org, 1);
					orgList.Add(org);
				} else {
					orgCount[org]++;
				}
			}
			
			orgList.Sort();
			buComboBox.Items.AddRange(orgList.ToArray());
			
			List<string> titleList = new List<string>();
			titleList.AddRange(titleCount.Keys);
			titleList.Sort(
				delegate(string x,string y) {
					if(getWeight(x) == getWeight(y)) {
						return 0;
					} else if(getWeight(x) > getWeight(y)) {
						return -1;
					} else {
						return 1;
					}
				});
			
			StringBuilder sb = new StringBuilder();
			int accCount = 0;
			foreach(string item in titleList) {
				if(item.Length > 0) {
					accCount += titleCount[item];
					float ratio = (float)titleCount[item] / employeeList.Count;
					if(ratio >= 0.001f) {
						sb.AppendLine(titleCount[item] + "\t(" + accCount + ")\t" + string.Format("{0:P}", ratio) + "\t" + item);
					} else {
						sb.AppendLine(titleCount[item] + "\t(" + accCount + ")\t----\t" + item);
					}
				}
			}
			
			sb.AppendLine("-----------------------------------------");
			orgList.Clear();
			orgList.AddRange(orgCount.Keys);
			orgList.Sort();

			foreach(string item in orgList) {
				sb.AppendLine(orgCount[item] + "\t" + item);
			}
			
			chagneTextBox.Text = sb.ToString();
		}

		private void analyzeYear(List<Employee> list) {
			Dictionary<int, Dictionary<String, int>> yearCount = new Dictionary<int, Dictionary<string, int>>();
			
			foreach(Employee item in list) {
				int year;
				
				if(item.alias.StartsWith("1")) {
					year = int.Parse(item.alias.Substring(0, 3));
				} else {
					year = int.Parse(item.alias.Substring(0, 2));
				}
				
				string title = item.title;
				if(title != "技術員") {
					title = title.Replace("技術", "").Replace("專案", "");
				}
				
				if(!yearCount.ContainsKey(year)) {
					yearCount.Add(year, new Dictionary<String, int>());
					yearCount[year].Add(title, 1);
				} else {
					
					if(!yearCount[year].ContainsKey(title)) {
						yearCount[year].Add(title, 1);
					} else {
						yearCount[year][title]++;
					}
				}
			}
			
			StringBuilder sb = new StringBuilder();
			List<int> yearList = new List<int>();
			yearList.AddRange(yearCount.Keys);
			yearList.Sort();
			yearList.Reverse();
			
			foreach(int year in yearList) {
				StringBuilder subSb = new StringBuilder();
				List<string> titleList = new List<string>();
				titleList.AddRange(yearCount[year].Keys);
				titleList.Sort(
					delegate(string x,string y) {
						if(getWeight(x) == getWeight(y)) {
							return 0;
						} else if(getWeight(x) > getWeight(y)) {
							return -1;
						} else {
							return 1;
						}
					});
				
				int count = 0;
				for(int i = 0; i < titleList.Count; i++) {
					subSb.AppendLine("\t" + titleList[i] + ": " + yearCount[year][titleList[i]]);
					count += yearCount[year][titleList[i]];
				}
				
				sb.Append(year + ": " + count);
				sb.AppendLine(subSb.ToString());
			}
			
			yearSummaryTextBox.Text = sb.ToString();
		}

		private void compare(List<Employee> list1,List<Employee> list2) {
			Dictionary<string, Employee> old = new Dictionary<string, MainForm.Employee>();
			Dictionary<string, Employee> current = new Dictionary<string, MainForm.Employee>();
			Dictionary<string, List<EmployeeChange>> changeList = new Dictionary<string, List<EmployeeChange>>();
			
			foreach(Employee item in list1) {
				old.Add(item.alias, item);
			}
			
			foreach(Employee item in list2) {
				current.Add(item.alias, item);
			}
			
			foreach(Employee item in list1) {
				if(!current.ContainsKey(item.alias)) {
					EmployeeChange change = new EmployeeChange();
					change.current = item;
					change.type = "QUIT";
					change.old = item.title;
					if(!changeList.ContainsKey(change.current.alias)) {
						changeList.Add(change.current.alias, new List<EmployeeChange>());
					}
					changeList[change.current.alias].Add(change);
				}
			}
			
			try {
				foreach(Employee item in list2) {
					if(!old.ContainsKey(item.alias)) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "NEW";

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
						continue;
					}
				
					if(old[item.alias].boss != item.boss) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "BOSS CNG";
						change.old = old[item.alias].boss;

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}

					if(old[item.alias].org != item.org) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "ORG CNG";
						change.old = old[item.alias].org;

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}

					if(old[item.alias].office != item.office) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "OFFICE CNG";
						change.old = old[item.alias].office;

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}
				
					if(old[item.alias].title != item.title) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "TITLE CNG";
						change.old = simpleTitle(old[item.alias].title);
						string currentTitle = simpleTitle(item.title);
					
						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}

					if(old[item.alias].email != item.email) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "EMAIL CNG";
						change.old = old[item.alias].email;

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}

					if(old[item.alias].name != item.name) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "NAME CNG";
						change.old = old[item.alias].name;

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}

					if(old.ContainsKey(item.alias) && old[item.alias].surname != item.surname) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "NAME CNG";
						change.old = old[item.alias].surname;

						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}

					if(old[item.alias].chtName != item.chtName) {
						EmployeeChange change = new EmployeeChange();
						change.current = item;
						change.type = "NAME CNG";
						change.old = old[item.alias].chtName;
	
						if(!changeList.ContainsKey(change.current.alias)) {
							changeList.Add(change.current.alias, new List<EmployeeChange>());
						}
						changeList[change.current.alias].Add(change);
					}
				}
			} catch(Exception e) {

			}
			int newer = 0;
			int quiter = 0;
			int bossCng = 0;
			int nameCng = 0;
			Dictionary<string, int> titleChange = new Dictionary<string, int>();
			Dictionary<string, int> titleChangeOrg = new Dictionary<string, int>();
			Dictionary<string, int> quitTitle = new Dictionary<string, int>();
			Dictionary<string, int> quitTitleOrg = new Dictionary<string, int>();
			Dictionary<string, int> newerTitle = new Dictionary<string, int>();
			Dictionary<string, int> newerTitleOrg = new Dictionary<string, int>();
			Dictionary<string, int> bossChgTitle = new Dictionary<string, int>();
			Dictionary<string, int> bossChgTitleOrg = new Dictionary<string, int>();
			
			try {
				foreach(List<EmployeeChange> item in changeList.Values) {
					foreach(EmployeeChange subitem in item) {
						if(subitem.type == "NEW") {
							newer++;
							if(subitem.current != null) {
								if(!newerTitle.ContainsKey(subitem.current.title)) {
									newerTitle.Add(subitem.current.title, 1);
								} else {
									newerTitle[subitem.current.title]++;
								}
							
								string org = simpleOrg(subitem.current.org);
								if(!newerTitleOrg.ContainsKey(org)) {
									newerTitleOrg.Add(org, 1);
								} else {
									newerTitleOrg[org]++;
								}
							}
						} else if(subitem.type == "QUIT") {
							quiter++;
							if(subitem.old != null) {
								if(!quitTitle.ContainsKey(subitem.old)) {
									quitTitle.Add(subitem.old, 1);
								} else {
									quitTitle[subitem.old]++;
								}
							
								string org = simpleOrg(subitem.current.org);
								if(!quitTitleOrg.ContainsKey(org)) {
									quitTitleOrg.Add(org, 1);
								} else {
									quitTitleOrg[org]++;
								}
							}
						} else if(subitem.type == "BOSS CNG") {
							bossCng++;
							if(subitem.current != null) {
								if(!bossChgTitle.ContainsKey(subitem.current.title)) {
									bossChgTitle.Add(subitem.current.title, 1);
								} else {
									bossChgTitle[subitem.current.title]++;
								}
							
								string org = simpleOrg(subitem.current.org);
								if(!bossChgTitleOrg.ContainsKey(org)) {
									bossChgTitleOrg.Add(org, 1);
								} else {
									bossChgTitleOrg[org]++;
								}
							}
						} else if(subitem.type == "NAME CNG") {
							nameCng++;
						} else if(subitem.type == "TITLE CNG") {
							string currentTitle = simpleTitle(subitem.current.title);
							if(subitem.old != currentTitle) {
								string str = currentTitle;
								string org = simpleOrg(subitem.current.org);
							
								if(!titleChangeOrg.ContainsKey(org)) {
									titleChangeOrg.Add(org, 1);
								} else {
									titleChangeOrg[org]++;
								}
							
								if(!titleChange.ContainsKey(str)) {
									titleChange.Add(str, 0);
								}							
								titleChange[str]++;
							} else {							
							}
						}
					}
				}
			} catch {				
			}
			
			try {
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("新來的: " + newer);
				sb.AppendLine(getTitleOrderCount(newerTitle));
				sb.AppendLine(getOrgDistr(newerTitleOrg));
				sb.AppendLine("-----------------------------------------");
				sb.AppendLine("走掉的: " + quiter);
				sb.AppendLine(getTitleOrderCount(quitTitle));
				sb.AppendLine(getOrgDistr(quitTitleOrg));
				sb.AppendLine("-----------------------------------------");
				sb.AppendLine("換名字: " + nameCng);
				sb.AppendLine("換老闆: " + bossCng);
				sb.AppendLine(getTitleOrderCount(bossChgTitle));
				sb.AppendLine(getOrgDistr(bossChgTitleOrg));
				sb.AppendLine("-----------------------------------------");
				int titleCngCount = 0;
				foreach(int item in titleChange.Values) {
					titleCngCount += item;
				}
				sb.AppendLine("升官: " + titleCngCount + " / " + old.Count + " (" + string.Format("{0:P}", (float)titleCngCount / old.Count) + ")");
				sb.AppendLine(getTitleOrderCount(titleChange));
				sb.AppendLine(getOrgDistr(titleChangeOrg));
				chagneTextBox.Text = sb.ToString();
				sb.AppendLine("-----------------------------------------");
			} catch {
			}
		}
		
		private string getOrgDistr(Dictionary<string, int> orgHash) {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("---------------------");
			List<KeyValuePair<string, int>> sortList = new List<KeyValuePair<string, int>>();
			foreach(string org in orgHash.Keys) {
				sortList.Add(new KeyValuePair<string, int>(org,orgHash[org]));
			}
			sortList.Sort(
				delegate(KeyValuePair<string, int> a,KeyValuePair<string, int> b) {
					float aV = (orgCount.ContainsKey(a.Key)?((float)(a.Value * 100 / orgCount[a.Key])):0f);
					float bV = (orgCount.ContainsKey(b.Key)?((float)(b.Value * 100 / orgCount[b.Key])):0f);
					return bV.CompareTo(aV);
				});
			foreach(KeyValuePair<string, int> item in sortList) {
				sb.Append("\t" + orgHash[item.Key]);
				if(orgCount != null && orgCount.ContainsKey(item.Key)) {
					sb.Append("\t" + orgCount[item.Key] + "\t" + string.Format("{0:P}", (float)orgHash[item.Key] / orgCount[item.Key]));
				} else {
					sb.Append("(----)");
				}
				sb.AppendLine("\t" + item.Key);
			}
			
			return sb.ToString();
		}
		
		private string getTitleOrderCount(Dictionary<string, int> list) {
			StringBuilder sb = new StringBuilder();
			List<string> newerTitleList = new List<string>();
			newerTitleList.AddRange(list.Keys);
			newerTitleList.Sort(
				delegate(string x,string y) {
					if(getWeight(x) == getWeight(y)) {
						return 0;
					} else if(getWeight(x) > getWeight(y)) {
						return -1;
					} else {
						return 1;
					}
				});
			int counter = 0;
			foreach(String item in newerTitleList) {
				counter += list[item];
				sb.AppendLine("\t" + item + ": " + list[item] + " (" + counter + ")");
			}
			
			return sb.ToString();
		}
		
		public class EmployeeChange {
			public Employee current;
			public string type;
			public string old;
		}
		
		private List<Employee> getEmployee(string path) {
			List<Employee> ret = new List<MainForm.Employee>();
			string content = File.ReadAllText(path);
			List<List<string>> data =
				Converter.csv2ListArray(File.ReadAllText(path));

			foreach(List<string> subitem in data) {
				Employee e = new Employee(
					             subitem[0],subitem[1],subitem[2],subitem[3],subitem[4],
					             subitem[5],subitem[6],subitem[7],subitem[8],subitem[9]);
				ret.Add(e);
			}
			
			return ret;
		}
		
		public static double getWeight(string t) {
			switch(t) {
				case "董事長":
				return 6;
				case "副董事長":
				return 5;
				case "CTO VP&GM":
				return 4;
				case "VP&GM":
				return 4;
				case "AVP&GM":
				return 4;
				case "Sr. VP&GM":
				return 4;
				case "資深副總經理":
				case "資深副總暨財務長":
				return 3.5;
				case "副總經理暨財務長":
				case "執行副總經理":
				return 3.2;
				case "副總經理":
				return 3;
				case "協理":
				case "技術協理":
				return 2.5;
				case "顧問":
				case "顧問A":
				case "顧問B":
				case "處長":
				case "技術處長":
				case "專案處長":
				case "研究處長":
				return 2.2;
				case "副處長":
				case "專案副處長":
				case "技術副處長":
				case "研究副處長":
				return 2.0;
				case "部經理":
				case "專案部經理":
				case "技術部經理":
				case "研究部經理":
				return 1.8;
				case "資深經理":
				case "專案資深經理":
				case "技術資深經理":
				case "研究資深經理":
				return 1.6;
				case "經理":
				case "專案經理":
				case "技術經理":
				case "研究經理":
				return 1.4;
				case "副理":
				case "技術副理":
				case "專案副理":
				case "研究副理":
				return 1.2;
				case "一級專員":
				case "研究一級專員":
				return 1.0;
				case "二級專員":
				case "研究二級專員":
				case "儲備幹部":
				return 0.9;
				case "資深組長":
				return 0.85;
				case "組長":
				case "工程師":
				case "研究工程師":
				case "管理師":
				case "研究管理師":
				return 0.8;
				case "助理工程師":
				case "助理管理師":
				case "管理員":
				return 0.6;
				case "技術員":
				return 0.5;
				case "助理技術員":
				case "助理員":
				return 0.4;
				case "工讀生":
				return 0.2;
			}
		
			return 0.0;
		}
		
		public class Employee {
			public string alias;
			public string email;
			public string phone;
			public string office;
			public string boss;
			public string name;
			public string surname;
			public string title;
			public string chtName;
			public string org;
			
			public Employee (string a, string e, string p, string of, string b,
			                 string n, string s, string t, string c, string or) {
				alias = a;
				email = e;
				phone = p;
				office = of;
				boss = b;
				name = n;
				surname = s;
				title = t;
				chtName = c;
				org = or;
			}
			
			public override string ToString() {
				string number = phone.Replace("(381)", "");
				return alias + " " + chtName + "(" + title + ") " + org + "(" + number + ")";
			}
		}
		
		void MainFormLoad(object sender,EventArgs e) {
		}
	}
}
