using LoggerClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelegatesDoneDifferent
{
    public partial class Part1 : Form
    {
        System.Collections.Specialized.NameValueCollection cfg = ConfigurationManager.AppSettings;

        ILogger lg;
        
        public Part1()
        {
            InitializeComponent();
            lg = new Logger((Logger.LogLevel)Enum.Parse(typeof(Logger.LogLevel), cfg["LogLevel"]), cfg["LogLocation"], "logDB.txt");
    
        }

        //Each button is wrapped in it's own region which refers the buttons actions and supporting code. 

        #region Example One
        private void btnExample_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Button Clicked");
        
        }
        #endregion

        #region Example Two
        private void btnExample2_Click(object sender, EventArgs e)
        {

            List<string> lst = new List<string>();

            lst.Add("Steve");
            lst.Add("Sam");
            lst.Add("Mark");

            string result = lst.Where( w => { return w == "Steve"; }).FirstOrDefault();
            //result = Steve

            result = lst.Where(SamFunction).FirstOrDefault();
            //result Sam

            result = lst.Where(MarkFunction).FirstOrDefault();
            //result Mark

            MessageBox.Show(result);
        }

        private bool SamFunction(string inval )
        {
            return inval == "Sam";
        
        }

        Func<string, bool> MarkFunction = (w) => { return w == "Mark"; };
        #endregion

        #region Example Three
        private void btnExample3_Click(object sender, EventArgs e)
        {
            //Action call no parameters
            MyMethod(() => { MessageBox.Show("Show Me"); });   

            //Action call with a string parameter
            string s = "SHOW ME";
            Action<string> ai = (val) => { MessageBox.Show(val); };
            MyMethod(ai,s);
            
            
            //Func call
            string retval = MyMethodString(() => { return "the Money";});
            MessageBox.Show(retval);

        }
        private void MyMethod(Action a)
        {
            //This is what invoke the delegate passed in
            a.Invoke();
        
        }

        private void MyMethod(Action<string> a, string msg)
        {
            //This is what invoke the delegate passed in
            a.Invoke(msg);

        }

        private string MyMethodString(Func<string> f)
        {
            //This is what invoke the delegate passed in
            return f.Invoke();
        }


        #endregion

        #region Example Four
        private void btnExample4_Click(object sender, EventArgs e)
        {
            WriteHeader();
            WriteBody();
            WriteFooter();
            MessageBox.Show("Examine test.txt file");
        }

        private void WriteHeader()
        {
            lg.Log("Start Header", "Program", Logger.LogLevel.Info);
            try 
	        {
                var fs = new StreamWriter("test.txt", true);
                fs.WriteLine("Header");
                fs.Close();
	        }
	        catch (Exception ex)
	        {
	            lg.Log(ex.Message,"Header",Logger.LogLevel.Error);	        }
            }

        private void WriteBody()
        {
            lg.Log("Start Body", "Program", Logger.LogLevel.Info);
            try 
	        {
                var fs = new StreamWriter("test.txt", true);
                fs.WriteLine("Body");
                fs.Close();
	        }
	        catch (Exception ex)
	        {
	            lg.Log(ex.Message,"Body",Logger.LogLevel.Error);	        
            }
        }
        private void WriteFooter()
        {
            lg.Log("Start Footer", "Program", Logger.LogLevel.Info);
            try 
	        {
                var fs = new StreamWriter("test.txt", true);
                fs.WriteLine("Footer");
                fs.Close();
	        }
	        catch (Exception ex)
	        {
	            lg.Log(ex.Message,"Footer",Logger.LogLevel.Error);	        
            }
        }

        #endregion

        #region Example Five
        private void btnExample5_Click(object sender, EventArgs e)
        {
            bool sugarhigh = true; //normally this would not be hard coded. Just for example

            WriteDocument wd = new WriteDocument(lg,"textasc.txt");
            wd.AddAction(new ActionDefinition() {dAction = WriteHeader, dDescription = "Header", dOrderRun = 1} );
            wd.AddAction(new ActionDefinition() {dAction = WriteBody, dDescription = "Body", dOrderRun = 2 });
            wd.AddAction(new ActionDefinition() {dAction = WriteFooter, dDescription = "Footer", dOrderRun = 3} );

            wd.Write(); //Write in ascending order

            WriteDocument wddesc = new WriteDocument(lg, "textdesc.txt");
            wddesc.AddAction(new ActionDefinition() { dAction = WriteHeader, dDescription = "Header", dOrderRun = 1 });
            wddesc.AddAction(new ActionDefinition() { dAction = WriteBody, dDescription = "Body", dOrderRun = 2 });
            //Notice I added a test to see if we needed to add a new section
            if (sugarhigh)
                wddesc.AddAction(new ActionDefinition() { dAction = WriteSugarHigh, dDescription = "HighSugar", dOrderRun = 2 });
            
            wddesc.AddAction(new ActionDefinition() { dAction = WriteFooter, dDescription = "Footer", dOrderRun = 3 });

            wddesc.Write(false); //Write in descending order

            MessageBox.Show("Examine textasc.txt and textdesc.txt files");
        }

        private void WriteHeader(StreamWriter fs)
        {
                fs.WriteLine("Header");
        }

        private void WriteBody(StreamWriter fs)
        {
                fs.WriteLine("Body");
        }
        private void WriteFooter(StreamWriter fs)
        {
                fs.WriteLine("Footer");
        }
        private void WriteSugarHigh(StreamWriter fs)
        {
            fs.WriteLine("No more sugar for you!");
        }
        #endregion

           
    }

    #region Example Helper Classes
    /// <summary>
    /// This class allows us to group more information abou the delegate together.
    /// You could add more properties like Classification and filter in the WriteDocument routine.
    /// </summary>
    class ActionDefinition
    { 
        /// <summary>
        /// The function to pass which accepts a StreamWriter as an argument
        /// </summary>
        public Action<StreamWriter> dAction { get; set; }
        /// <summary>
        ///The description, I use for logging the function invoked 
        /// </summary>
        public string dDescription { get; set; }
        /// <summary>
        /// The order number, can be used to re-order the execution.
        /// </summary>
        public int dOrderRun { get; set; }

    }

    class WriteDocument
    {
        ILogger lg;
        string fname;
        
        //Pass in reference to logger and filename
        public WriteDocument(ILogger log,string filename)
        {
            lg = log;
            fname = filename;
        }

        //Create a list of action definitions which will be invoked
        private List<ActionDefinition> la = new List<ActionDefinition>();

        //Used to add actions to the list
        internal void AddAction(ActionDefinition a)
        {
            la.Add(a);
        }

        /// <summary>
        /// Write out the data using delegates, in ascending order
        /// </summary>
        internal void Write()
        {
            Write(true);
        }
        
        /// <summary>
        /// Write out the data using delegates
        /// </summary>
        /// <param name="orderdirectionasc">True = Acending, False Descending</param>
        internal void Write(bool orderdirectionasc)
        {
            lg.Log("WriteDocument Start", "WriteDocument", Logger.LogLevel.Info);
            try
            {
                //notice that now I don't need to keep appending file, doing all my work at one time, so we can overwrite each time.
                using (var fs = new StreamWriter(fname, false))
                {
                    //Notice we can change order of processing by specifying.
                    List<ActionDefinition> lordered;
                    if (orderdirectionasc)
                        lordered = la.OrderBy( o => o.dOrderRun).ToList();
                    else
                        lordered = la.OrderByDescending( o => o.dOrderRun).ToList();
                    
                    foreach (var action in lordered)
                    {
                        //Notice we invoke the dAction set in the class.
                        action.dAction.Invoke(fs); 
                        //We can log the name of the method (sure I could have used reflection as well)
                        lg.Log(action.dDescription, "WriteDocument", Logger.LogLevel.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                //We can log and change the Exception thrown
                lg.Log(ex.Message, "WriteDocument", Logger.LogLevel.Error);
                throw new Exception("An Error occured please check your logs");
            }
            finally
            {
                lg.Log("WriteDocument End", "WriteDocument", Logger.LogLevel.Info);
            }

        }
    #endregion

    }

   

}
