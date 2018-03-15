using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ActionFramework
{
    public class TestRunner : ITestRunner
    {
        public List<Test> RunTests(string pathToTestLibrary)
        {
            var listOfTestResults = new List<Test>();

            var assembly = Assembly.LoadFrom(pathToTestLibrary);
            var types = assembly.GetTypes();
            var testClasses = types.Where(x => x.IsClass && x.IsPublic && x.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(TestClassAttribute)));
            foreach (var testClass in testClasses)
            {
                var methods = testClass.GetMethods();
                var testMethods = methods.Where(x => x.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(TestMethodAttribute)));
                if (testMethods.Any())
                {
                    var instanceOfTestClass = Activator.CreateInstance(testClass);
                    foreach (var testMethod in testMethods)
                    {
                        var testResult = new Test();
                        testResult.Name = string.Format("{0}.{1}", testClass.Name, testMethod.Name);
                        try
                        {
                            testMethod.Invoke(instanceOfTestClass, null);
                            testResult.Success = true;
                        }
                        catch (TestException testException)
                        {
                            testResult.Success = false;
                            testResult.ErrorMessage = string.Format("Test exception: {0}", testException.Message);
                        }
                        catch (Exception exception)
                        {
                            testResult.Success = false;
                            testResult.ErrorMessage = string.Format("{0}:{1}", exception.GetType().Name, exception.Message);
                        }

                        listOfTestResults.Add(testResult);
                    }
                }
            }

            return listOfTestResults;
        }
    }
}
