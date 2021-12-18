using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestHarness
{
    class Program
    {
        const string InformationAttributeTypeName = "UtilityFunctions.InformationAttribute";

        static void Main(string[] args)
        {
            const string TargetAssemblyFileName = "UtilityFunctions.dll";
            const string TargetNamespace = "UtilityFunctions";

            Assembly assembly = Assembly.LoadFile(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    TargetAssemblyFileName
            ));


            List<Type> classes = assembly.GetTypes()
                .Where(t => t.Namespace == TargetNamespace && HasInformationAtrribute(t))
                .ToList();


            while (true)
            {
                Console.Clear();

                WritePromptToScreen("please enter number key associated with the class you wish to test:");
                DisplayProgramElementList(classes);
                Type typeChoice = ReturnProgramElementReferenceFromList(classes);
                Console.Clear();
                WriteHeadingsToScreen($"class choice: '{typeChoice}'");
                DisplayElementDescription(GetInformationAttributeDescriptionAtrribute(typeChoice));

                WritePromptToScreen("please enter method u want to test");
                List<MethodInfo> methods = typeChoice.GetMethods().Where(t => HasInformationAtrribute(t)).ToList();
                DisplayProgramElementList(methods);
                MethodInfo methodChoice = ReturnProgramElementReferenceFromList(methods);

                if (methodChoice != null)
                {
                    Console.Clear();
                    WriteHeadingsToScreen($"class choice: '{typeChoice}' & Method choice: '{methodChoice}'");
                    DisplayElementDescription(GetInformationAttributeDescriptionAtrribute(methodChoice));

                    ParameterInfo[] parmeters = methodChoice.GetParameters();
                    object classInstance = Activator.CreateInstance(typeChoice, null);
                    object result = GetResult(classInstance, methodChoice, parmeters);
                    WriteResultToScreen(result);
                }

                WritePromptToScreen("please enter 'spacebar' to end the application or any other key to continue...");
                if(Console.ReadKey().Key == ConsoleKey.Spacebar)
                {
                    break;
                }
            }
        }

        private static string GetInformationAttributeDescriptionAtrribute(MemberInfo memberInfo)
        {
            const string InformationAttributeDescriptionPropName = "Description";

            foreach (var attrib in memberInfo.GetCustomAttributes())
            {
                Type typeOfAttrib = attrib.GetType();
                if (typeOfAttrib.ToString().ToUpperInvariant() == InformationAttributeTypeName.ToUpperInvariant())
                {
                    PropertyInfo propertyInfo = typeOfAttrib.GetProperty(InformationAttributeDescriptionPropName);
                    if(propertyInfo != null)
                    {
                        object s = propertyInfo.GetValue(attrib, null);
                        if(s != null)
                        {
                            return s.ToString();
                        }
                    }
                }
            }

            return null;
        }

        private static void DisplayElementDescription(string elementDescription)
        {
            if(elementDescription != null)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(elementDescription);
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private static bool HasInformationAtrribute(MemberInfo memberInfo)
        {
            foreach(var attrib in memberInfo.GetCustomAttributes())
            {
                Type typeOfAttrib = attrib.GetType();
                if(typeOfAttrib.ToString().ToUpperInvariant() == InformationAttributeTypeName.ToUpperInvariant())
                {
                    return true;
                }
            }

            return false;
        }

        private static void WriteResultToScreen(object result)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Result: {result}");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static object[] ReturnParameterValuesInputAsObjectArray(ParameterInfo[] parameters)
        {
            object[] parameterValues = new object[parameters.Length];

            int itemCount = 0;
            foreach(ParameterInfo parameterInfo in parameters)
            {
                WritePromptToScreen($"please enter value for the parameter named: {parameterInfo.Name}");
                if(parameterInfo.ParameterType == typeof(string))
                {
                    string input = Console.ReadLine();
                    parameterValues[itemCount] = input;
                }
                else if(parameterInfo.ParameterType == typeof(double))
                {
                    double input = double.Parse(Console.ReadLine());
                    parameterValues[itemCount] = input;
                }
                else if(parameterInfo.ParameterType == typeof(int))
                {
                    int input = int.Parse(Console.ReadLine());
                    parameterValues[itemCount] = input;
                }

                itemCount++;
            }

            return parameterValues;
        }

        private static object GetResult(
            object classInstance,
            MethodInfo methodInfo,
            ParameterInfo[] parameters)
        {
            object result = null;

            if(parameters.Length == 0)
            {
                result = methodInfo.Invoke(classInstance, null);
            }
            else
            {
                var paramValuesList = ReturnParameterValuesInputAsObjectArray(parameters);
                result = methodInfo.Invoke(classInstance, paramValuesList);
            }

            return result;
        }

        private static T ReturnProgramElementReferenceFromList<T>(List<T> classes)
        {
            ConsoleKey consoleKey = Console.ReadKey().Key;

            switch (consoleKey)
            {
                case ConsoleKey.D1:
                    return classes[0];
                case ConsoleKey.D2:
                    return classes[1];
                case ConsoleKey.D3:
                    return classes[2];
                case ConsoleKey.D4:
                    return classes[3];
                case ConsoleKey.D5:
                    return classes[4];
            }

            return default;
        }

        private static void DisplayProgramElementList<T>(List<T> list)
        {
            int count = 0;
            foreach(var item in list)
            {
                count++;
                Console.WriteLine($"{count}. {item}");
            }
        }

        private static void WriteHeadingsToScreen(string heading)
        {
            Console.WriteLine(heading);
            Console.WriteLine(new string('-', heading.Length));
            Console.WriteLine();
        }

        private static void WritePromptToScreen(string proptText)
        {
            Console.WriteLine(proptText);
        }
    }
}
