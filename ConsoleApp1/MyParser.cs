using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MyParser
    {
        char[] set =     { '0','1','2','3','4','5','6','7','8','9','.',',','-','+','\n' };

        short[,] graf = { { 2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  3,  3,  1,  1 , 5},
                          { 2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  3,  3,  5,  5 , 5},
                          { 2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  3,  3,  5,  5 , 6},
                          { 4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  5,  5,  5,  5 , 7},
                          { 4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  5,  5,  5,  5 , 7} };

        public MyParser() 
        {
            errorMessage = "Ошибок нет.";
        }


        
        string errorMessage;
        public string ErrorMessage { get { return errorMessage; }}

        
        public double Parse(string input) 
        {
            double result = 0;
            short sign = 1; // знак числа
            double left = 0;
            double right = 0;

            short step = 0;
            short nextStep = 0;

            short index = 0; // позиция в строке
            
            
            List<short> stack = new List<short>();
            int y = 1;
            short digit = 0;
            char sym = '\n';

            while (step != 8) 
            {
                if (step == 5 || step == 6 || step == 7) nextStep = 8;
                else 
                {
                    if (index >= input.Count()) sym = '\n';
                    else sym = input[index];

                    short i = 0;
                    while (i < 15)
                    {
                        if ( sym == set[i])
                        {
                            nextStep = graf[step, i];
                            break;
                        }
                        i++;
                    }
                    if (i < 10) digit = i;
                    if (i == 15) nextStep = 5;
                }

                

                switch (step)
                {
                    case 0: // есть ли знак
                        if (sym == '-') sign = -1;
                        if (nextStep == 1 || nextStep == 3) index++;
                        break;

                    case 1: // что за знаком
                        if (nextStep == 3) index++;
                        break;

                    case 2: // собираем левую часть
                        if (nextStep == 2) { stack.Add(digit); index++; }
                        if (nextStep == 3 || nextStep == 6)
                        {
                            double x = 0;
                            while (stack.Count > 0)
                            {
                               
                                left += stack.Last() * Math.Pow(10, x);
                                
                                if(double.IsInfinity(left))
                                {
                                    errorMessage = $"Ошибка: переполнение.";                                   
                                }
                                
                                x++;
                                stack.RemoveAt(stack.Count - 1);
                            }
                            index++;
                        }
                        break;

                    case 3: // найдена точка                       
                        break;

                    case 4: // собираем правую часть
                        if (nextStep == 4) 
                        {                            
                            right += digit / Math.Pow(10, y);
                            y++;
                            index++; 
                        }
                        break;

                    case 5: // ошибка. неверный символ
                        errorMessage = $"Ошибка: неверный символ (позиция {index}).";
                        break;

                    case 6: // результат без дробной части

                        result = sign * left;
                        if (double.IsInfinity(result))
                        {
                            errorMessage = $"Ошибка: переполнение.";                            
                        }

                        break;
                    case 7: // результат с дробной частью

                        result = sign * (left + right);
                        if (double.IsInfinity(result))
                        {
                            errorMessage = $"Ошибка: переполнение.";                            
                        }

                        break;

                    default:
                        break;

                }

                step = nextStep;
                
            }

            return result;
        }
    }
}
