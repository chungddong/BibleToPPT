using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using TextRange = Microsoft.Office.Interop.PowerPoint.TextRange;
using Application = System.Windows.Application;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using Style = System.Windows.Style;
using Window = System.Windows.Window;
using Syncfusion.Drawing;
using Color = Syncfusion.Drawing.Color;
using System.Diagnostics;

namespace BibleToPPT
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        string[] volumes = new string[] { "창세기", "출애굽기", "레위기", "민수기", "신명기", "여호수아", 
            "사사기", "룻기", "사무엘상", "사무엘하", "열왕기상", "열왕기하", "역대상", "역대하", "에스라", 
            "느헤미야", "에스더", "욥기", "시편", "잠언", "전도서", "아가", "이사야", "예레미야", "예레미야애가", 
            "에스겔", "다니엘", "호세아", "요엘", "아모스", "오바댜", "요나", "미가", "나훔", "하박국", "스바냐", 
            "학개", "스가랴", "말라기", "마태복음", "마가복음", "누가복음", "요한복음", "사도행전", "로마서", 
            "고린도전서", "고린도후서", "갈라디아서", "에베소서", "빌립보서", "골로새서", "데살로니가전서",
            "데살로니가후서", "디모데전서", "디모데후서", "디도서", "빌레몬서", "히브리서", "야고보서", "베드로전서",
            "베드로후서", "요한일서", "요한이서", "요한삼서", "유다서", "요한계시록"};

        string biblepath = @"Bibles\1-01창세기.txt";
        string bibleName = "창세기";

        public MainWindow()
        {
            InitializeComponent();
            CreateBtns(); //버튼 생성
            CreatePageBtns(50);

            //파일 잘 나오는지 테스트한거

            /*System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("Bibles");

            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                //
                //Debug.WriteLine("파일명 : " + file.Name);
                MessageBox.Show("파일명 : " + file.Name);
            }*/
            page = "1";


            firstload();

        }

        private void firstload()
        {
            StreamReader sss = new StreamReader(biblepath, System.Text.Encoding.GetEncoding(949));

            string line;

            string info;
            string linenum;

            int maxverse = 0;

            int maxpage = 0;

            while ((line = sss.ReadLine()) != null)
            {
                info = line.Substring(0, line.IndexOf('\x020'));   //콘텐츠 정보
                linenum = info.Substring(line.IndexOf(':') + 1); //몇 절인지 확인

                string[] temp;
                temp = info.Split(':');
                info = temp[0];
                info = Regex.Replace(info, @"\D", "");

                if(int.Parse(info) > maxpage)
                {
                    maxpage = int.Parse(info);
                }

                if (info == "1")
                {

                    if (int.Parse(linenum) > maxverse)
                    {
                        maxverse = int.Parse(linenum);
                    }

                }
            }
            sss.Close();

            verseStart.Text = "1";
            verseEnd.Text = maxverse.ToString();
            CreatePageBtns(maxpage);
            //MessageBox.Show(bibleName + "는 " + maxpage + " 장까지 있습니다");
        }

        private void bibleClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("" + ((RadioButton)sender).Content.ToString());

            page = "1";

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("Bibles");

            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                //
                //Debug.WriteLine("파일명 : " + file.Name);
                //MessageBox.Show("파일명 : " + file.Name);

                bool iscontain = file.Name.Contains(((RadioButton)sender).Content.ToString());

                bibleName = ((RadioButton)sender).Content.ToString();

                if (iscontain == true)
                {
                    biblepath = @"Bibles\" + file.Name;
                    //MessageBox.Show(biblepath);
                }
            }

            firstload();



        }




        private void Window_mouseDown(object sender,MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((Button)sender).Name.ToString());
            
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void CreateBtns()
        {
            for(int j = 0; j < volumes.Length; j++)
            {
                RadioButton btn = new RadioButton();
                //btn.Name = "Volume" + j;
                btn.Name = volumes[j].ToString();

                if(j == 0)
                {
                    btn.IsChecked = true;
                }

                btn.Content = btn.Name;
                btn.Tag = j;   // tag
                btn.Height = 50;   // 세로길이
                btn.Width = 170;   // 가로길이
                btn.Margin = new System.Windows.Thickness { Bottom = 6 };
                
                btn.Click += new RoutedEventHandler(bibleClick);

                btn.Style = (Style)Application.Current.FindResource("PrimaryButtonStyle"); //스타일 가져오기

                wraps.Children.Add(btn);   // grid1 에 넣기
            }
        }

        private void CreatePageBtns(int maxpage)
        {
            PageWraps.Children.Clear();
            for (int j = 1; j < maxpage + 1; j++)
            {
                RadioButton btn = new RadioButton();
                //btn.Name = "Volume" + j;
                btn.Name = "page" + j;

                if (j == 1)
                {
                    btn.IsChecked = true;
                }

                btn.Content = j;
                btn.Tag = j;   // tag
                btn.Height = 40;   // 세로길이
                btn.Width = 40;   // 가로길이
                btn.Margin = new System.Windows.Thickness { Bottom = 10 };
                btn.Click += new RoutedEventHandler(PageButtonTest);

                btn.Style = (Style)Application.Current.FindResource("PageButtonStyle"); //스타일 가져오기

                PageWraps.Children.Add(btn);   // grid1 에 넣기
            }
        }

        string page;

        private void PageButtonTest(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((RadioButton)sender).Content.ToString());
            page = ((RadioButton)sender).Content.ToString();

            textbox.Clear();

            //MessageBox.Show(biblepath);

            StreamReader sss = new StreamReader(biblepath, System.Text.Encoding.GetEncoding(949));

            string line;

            string info;
            string linenum;

            int maxverse = 0;

            


            while ((line = sss.ReadLine()) != null)
            {
                info = line.Substring(0, line.IndexOf('\x020'));   //콘텐츠 정보
                linenum = info.Substring(line.IndexOf(':') + 1); //몇 절인지 확인
                
                string[] temp;
                temp = info.Split(':');
                info = temp[0];
                info = Regex.Replace(info, @"\D", "");

                if (info == ((RadioButton)sender).Content.ToString())
                {

                    if (int.Parse(linenum) > maxverse)
                    {
                        maxverse = int.Parse(linenum);
                    }

                }
            }
            sss.Close();

            verseStart.Text = "1";
            verseEnd.Text = maxverse.ToString();
            
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        int startnum, endnum;


        private void verseStartChanged(object sender, TextChangedEventArgs e)
        {
            if (verseStart.Text.Length > 0)
            {
                startnum = int.Parse(verseStart.Text);

                textbox.Clear();

                StreamReader sss = new StreamReader(biblepath, System.Text.Encoding.GetEncoding(949));

                string line;
                string result = "";

                string info;
                string linenum;

                result += bibleName + page + "장"; //맨 윗줄에 장 수 보이게 하기
                result += "\r\n";

                while ((line = sss.ReadLine()) != null)
                {
                    info = line.Substring(0, line.IndexOf('\x020'));   //콘텐츠 정보 ex) 창5:2
                    linenum = info.Substring(line.IndexOf(':') + 1); //몇 절인지 확인

                    line = line.Substring(line.IndexOf('\x020') + 1); //글자들 자른거

                    string[] temp;
                    temp = info.Split(':'); //창5, 2
                    info = temp[0]; //창5
                    info = Regex.Replace(info, @"\D", ""); //5

                    if (info == page)
                    {
                        if (int.Parse(linenum) >= startnum && int.Parse(linenum) <= endnum)
                        {
                            result += linenum +"." + line; // + info <== 테스트용
                            result += "\r\n";

                            tempdata[int.Parse(linenum)] = line;
                        }

                    }
                }
                textbox.Text = result;
                sss.Close();
            }

        }


        
        private void verseEndChanged(object sender, TextChangedEventArgs e)
        {
            if (verseEnd.Text.Length > 0)
            {
                endnum = int.Parse(verseEnd.Text);

                if (endnum > startnum)
                {
                    //MessageBox.Show("입력 범위 : " + startnum + " ~ " + endnum);

                    startnum = int.Parse(verseStart.Text);

                    textbox.Clear();

                    StreamReader sss = new StreamReader(biblepath, System.Text.Encoding.GetEncoding(949));

                    string line;
                    string result = "";

                    string info;
                    string linenum;

                    result += bibleName + page + "장"; //맨 윗줄에 장 수 보이게 하기
                    result += "\r\n";

                    while ((line = sss.ReadLine()) != null)
                    {
                        info = line.Substring(0, line.IndexOf('\x020'));   //콘텐츠 정보
                        linenum = info.Substring(line.IndexOf(':') + 1); //몇 절인지 확인

                        line = line.Substring(line.IndexOf('\x020') + 1); //글자들 자른거

                        string[] temp;
                        temp = info.Split(':');
                        info = temp[0];
                        info = Regex.Replace(info, @"\D", "");

                        if (info == page)
                        {
                            if (int.Parse(linenum) >= startnum && int.Parse(linenum) <= endnum)
                            {
                                result += linenum + "." + line; // + info <== 테스트용
                                result += "\r\n";

                                tempdata[int.Parse(linenum)] = line;
                            }
                                
                        }
                    }
                    textbox.Text = result;
                    sss.Close();
                }
            }



        }

        string[] tempdata = new string[1000];

        bool isbackground = false;

        private void customTemplate(object sender, RoutedEventArgs e)
        {
            BibleToPPT.customTemplate ct = new BibleToPPT.customTemplate();
            //ct.Show();
            ct.ShowDialog();
        }

        private void checkbox_chage(object sender, RoutedEventArgs e)
        {
            if(checkbox.IsChecked != true)
            {

            }
            else
            {
                
            }
        }

        private void ppt_Test(object sender, RoutedEventArgs e)
        {
            if(checkbox.IsChecked == true)
            {
                isbackground = true;
            }
            else
            {
                isbackground = false;
            }

            Microsoft.Office.Interop.PowerPoint.Application pptApplication = new Microsoft.Office.Interop.PowerPoint.Application();

            Microsoft.Office.Interop.PowerPoint.Slides slides;
            Microsoft.Office.Interop.PowerPoint.Slides[] slidenum;
            Microsoft.Office.Interop.PowerPoint._Slide slide;
            Microsoft.Office.Interop.PowerPoint.TextRange objText;

            // 프레젠테이션 파일 생성
            Presentation pptPresentation = pptApplication.Presentations.Add(MsoTriState.msoTrue);

            Microsoft.Office.Interop.PowerPoint.CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutText];

            // Create new Slide
            /*slides = pptPresentation.Slides;
            slide = slides.AddSlide(1, customLayout);*/

            slidenum = new Slides[1000];  //슬라이드 넣을 배열 선언

            float slideWidth = pptPresentation.PageSetup.SlideWidth;
            float slideHeight = pptPresentation.PageSetup.SlideHeight;

            //pptPresentation.PageSetup.SlideWidth = (slideHeight * (4 / 3));
            pptPresentation.PageSetup.SlideWidth = 1024;
            pptPresentation.PageSetup.SlideHeight = 768;

            /*CustomLayout customLayouts = pptPresentation.SlideMaster.CustomLayouts[7];

            customLayouts.DisplayMasterShapes = MsoTriState.msoFalse;


            customLayouts.Shapes.AddTitle();
            //customLayouts.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, 0, 200, 200);
            //customLayouts.Shapes.AddPlaceholder(PpPlaceholderType.ppPlaceholderObject, 0, 500, 300, 300);
            //customLayouts.Shapes.AddPlaceholder(PpPlaceholderType.ppPlaceholderBody, 500, 0, 200, 200);

            slides = pptPresentation.Slides;
            slide = slides.AddSlide(1, customLayouts);*/

            string strPath = System.Environment.CurrentDirectory.ToString();
            //MessageBox.Show("" + strPath);

            try
            {
                for (int i = endnum; i >= startnum; i--)
                {
                    slidenum[i] = pptPresentation.Slides;
                    slide = slidenum[i].AddSlide(1, customLayout);
                    
                    //slide.BackgroundStyle = MsoBackgroundStyleIndex.msoBackgroundStylePreset4;
                    slide.FollowMasterBackground = Microsoft.Office.Core.MsoTriState.msoFalse;

                    if(isbackground)
                    {
                        slide.Background.Fill.UserPicture(strPath + @"\bible.jpg");
                    }
                    else
                    {
                        slide.BackgroundStyle = MsoBackgroundStyleIndex.msoBackgroundStylePreset4;
                    }

                    //slide.Background.Fill.UserPicture("bible.jpg");

                    //slide.Shapes.AddLabel(MsoTextOrientation.msoTextOrientationHorizontal, 50, 40, 40, 40);


                    //제목
                    objText = slide.Shapes[1].TextFrame.TextRange;
                    //objText.Font.Color.SchemeColor = PpColorSchemeIndex.ppFill;
                    objText.Font.Color.RGB = Color.White.ToArgb();
                    objText.Text = bibleName + " " + page + "장 " + i + "절";
                    objText.Font.Name = "Arial";
                    objText.Font.Bold = MsoTriState.msoCTrue;
                    objText.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter;
                    objText.Font.Size = 52;

                    //서브 텍스트
                    objText = slide.Shapes[2].TextFrame.TextRange;
                    objText.Text = tempdata[i].ToString();
                    objText.Font.Color.RGB = Color.White.ToArgb();
                    objText.Font.Bold = MsoTriState.msoCTrue;
                    objText.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter;
                    objText.Font.Size = 60;

                    //slide.NotesPage.Shapes[2].TextFrame.TextRange.Text = "자동으로 PPT 생성"; //슬라이드 노트
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
            }

            /*string[] temp;
            temp = info.Split(':'); //창5, 2
            info = temp[0]; //창5
            info = Regex.Replace(info, @"\D", ""); //5*/

            

            //pptPresentation.SaveAs(@"c:\temp\fppt.pptx", Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
            //pptPresentation.Close();
            //pptApplication.Quit();


        }


    }
}
