# Delegates-Part-1
Examples of delegate usage beyond event handling.

## Introduction
This is an article about using delegates in a way in which you may not have thought of using
them before. This is an overview of delegates and how to group functionality and cross cutting concerns with them. 

## Background
A delegate is defined as a type safe function which can be passed around just like a
reference variable (pointer to a memory space) and can be used to inject functionality into an
object.

If you have done Winforms development, you have already used delegates in the form of
event handlers.

Assume you have a button called btnExample_Click.
In a partial class created by designer, you will find the following and in the CS file, you will
find the referenced code.

```csharp
//in Part1.Designer.cs
this.btnExample.Click += new System.EventHandler(this.btnExample_Click);
//in Part1.cs
private void btnExample_Click(object sender, EventArgs e)
{
}
```
System.EventHandler() is defined to take a predefined delegate called void
EventHandler(object sender, EventArgs e) where object is the object which
generated the event (in this case, the button) and EventArgs is defined as a class which
allows you to pass data specific to the object which generated the event.


If you have used Linq extensions, you have also used delegates in the form of functions
passed to do something on behalf of the extension.

```csharp
 private void btnExample2_Click(object sender, EventArgs e)
 {
     List<string> lst = new List<string>();
     lst.Add("Steve"); 
     lst.Add("Sam");
     lst.Add("Mark");
     string result = lst.Where( w => { return w == "Steve";
    }).FirstOrDefault();
     //result = Steve
     result = lst.Where(SamFunction).FirstOrDefault();
     //result Sam
     result = lst.Where(MarkFunction).FirstOrDefault();
     //result Mark
 }
```
You may have seen a signature that looks like:
```csharp
  Lst.Where(Func<TSource, bool> predicate)
```
The Where extension is asking for a function which takes an item of the list and returns true
or false depending on the object passed in. The parameter can be filled in a number of
ways:

As a Lambda Expression:
```csharp
  string result = lst.Where( w => { return w == "Steve"; }).FirstOrDefault(); 
  //result = Steve
```
Or as a function passed in:
```csharp
  result = lst.Where(SamFunction).FirstOrDefault(); - //result Sam
```

Where Sam function is defined as:
```csharp
private bool SamFunction(string inval )
{
 return inval == "Sam";
}
```
In all cases, the function returns true or false depending on item passed in.

## So the question is, can you create your own functions which use other functions inside of them? 
There are two special keywords:

Action and Func - which are delegates that can be used without explicitly declaring a custom delegate.

Action is used to pass void functions (sub routines in VB.NET) and Func allow you to pass functions (function in VB.NET) which return a value.


Each delegate type is overloaded to accept a different number (up to 16) of input parameters and in the case of Func, TResult to return only one parameter.

Example:
> Action(T) accepts one parameter of generic T type.

> Action(T1, T2) accepts two parameters of generic T type.

> Func(T, TResult) accepts one parameter of generic T type and returns a gener

> Func(T1, T2, TResult) accepts two parameters of generic T type and returns a


The last parameter (TResult) in a Func delegate is the type of return.

In the following, I am creating three functions which accept either an Action or Func
delegate as a parameter.
```csharp
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

//Example use:

private void btnExample3_Click(object sender, EventArgs e) 
 {
 //Action call no parameters
 MyMethod(() => { MessageBox.Show("Show Me"); });
 //Action call with a string parameter
 string s = "SHOW ME";
 //Notice assignment to a Action(T) delegate
 Action<string> ai = (val) => { MessageBox.Show(val); };
 MyMethod(ai,s);

 //Func call
 string retval = MyMethodString(() => { return "the Money";});
 MessageBox.Show(retval);
 }
```

The question is then, what other things can we do with them?

One idea is to use them to group cross cutting concerns into one place:

How many times have you written functions which look something like the following?

```csharp
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
     lg.Log(ex.Message,"Header",Logger.LogLevel.Error);
   }
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
```
Maybe more of these…

Everything is working fine, then your boss says, “Just went through the logs and realized I
never told you that you should also log when the function exits. And by the way, you should
be wrapping your code in a using statement.”

Now you have to go to X number of functions and add x.log(“end”) to the bottom and
wrap in a using statement.

Now the boss says: “I recently went to my re-union and met an old friend who told me all
about handling errors and re-throwing friendly messages.”

Over time, each of your functions grow and are progressively harder to maintain. 

The next developer to support the code doesn’t know which methods need to be modified for the
next request. The issues compound.

What if you could execute all these requests in one place and defer execution until you’re
ready? That is what we will do using delegates.

### (Region 5 in Code and Example Helper Classes)
What I have done is to create a class which I can add the delegates and execute and invoke in one place. The execution is deferred until I call the method to write.
#### (Region Example Helper Classes)
```csharp
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
   lg.Log("WriteDocument Start", "WriteDocument",
   Logger.LogLevel.Info);
 try
 {
   //notice that now I don't need to keep appending file, doing all my work at one time,
   //so we can overwrite each time.
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
       action.d=Action.Invoke(fs);
       //We can log the name of the method (sure I could have used reflection as well)
       lg.Log(action.dDescription, "WriteDocument",
      Logger.LogLevel.Info);
     }
   }
 }
   catch (Exception ex)
   {
   //We can log and change the Exception thrown
   lg.Log(ex.Message, "WriteDocument", Logger.LogLevel.Error);
   throw new Exception("An Error occurred please check your logs");
  
   } 
   finally
   {
   lg.Log("WriteDocument End", "WriteDocument",
   Logger.LogLevel.Info);
   }
 }
```
Notice it looks a lot like what we have in each function. In addition, the boss asked if we
could print both forward and backword. Something we could not easily do before. :)


It uses a supporting class to hold information about the function being added and invoked:

```csharp
     /// <summary>
     /// This class allows us to group more information about the delegate
    together.
     /// You could add more properties like Classification and filter in the
    WriteDocument routine.
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
```
### (Region 5 in Code)

Boss Alert! "I want to add a section to print, but ONLY if a test comes back positive." 
It is set up in the following way:
```csharp
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
 private void btnExample5_Click(object sender, EventArgs e)
 {
     bool sugarhigh = true; //normally this would not be hard coded. Just for example
     WriteDocument wd = new WriteDocument(lg,"textasc.txt");
     wd.AddAction(new ActionDefinition() {dAction = WriteHeader,
     dDescription = "Header", dOrderRun = 1} );
     wd.AddAction(new ActionDefinition() {dAction = WriteBody,
     dDescription = "Body", dOrderRun = 2 });
     wd.AddAction(new ActionDefinition() {dAction = WriteFooter,
     dDescription = "Footer", dOrderRun = 3} );
     wd.Write(); //Write in ascending order
     WriteDocument wddesc = new WriteDocument(lg, "textdesc.txt");
     wddesc.AddAction(new ActionDefinition() { dAction = WriteHeader, dDescription = "Header", dOrderRun = 1 });
     wddesc.AddAction(new ActionDefinition() { dAction = WriteBody,  dDescription = "Body", dOrderRun = 2 });
     //Notice I added a test to see if we needed to add a new section
     if (sugarhigh)
         wddesc.AddAction(new ActionDefinition() { dAction = WriteSugarHigh, dDescription = "HighSugar", dOrderRun = 2 });

      wddesc.AddAction(new ActionDefinition() { dAction = WriteFooter, dDescription = "Footer", dOrderRun = 3 });
     wddesc.Write(false); //Write in descending order
 }
```
Three files are created:
Log File, Testasc.txt and testdesc.txt.

Although there is some setup to do, there is a lot of functionality we could not do before.

This example would be very useful when dynamically creating certain sections of a
document depending on other types of data, for example, a section is inserted in a document
because a blood test came back that your sugar was high. In addition, you can print
backwards as well!

```csharp
  //Notice I added a test to see if we needed to add a new section
  if (sugarhigh)
     wddesc.AddAction(new ActionDefinition() { dAction = WriteSugarHigh, dDescription = "HighSugar", dOrderRun = 2 });

```
In the example, the descending file will have an extra line for sugar high.

#### Points of Interest
In addition to examples illustrated here, I have also used this technique to execute blocks of
code which need to be included in a SQL transaction, committing when all blocks are
successful or rolling back when any one fails.

#### When debugging 
It helps to separate the Lambda expression over several lines, so you can 
set a break point more easily. Another trick is to write as a named function, debug your logic
and convert to an anonymous function later.
