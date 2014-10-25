using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.BinarySearch
{
    public class AutoLoan
    {
        public double InterestRate(double price, double monthlyPayment, int loanTerm)
        {
            double low = 0;
            double high = 100; //max interest rate
            double mid;

            while (high - low > 1e-9)
            {
                mid = low + (high - low) / 2;

                if (_interestRateIsHigh(price, monthlyPayment, loanTerm, mid))
                {
                    high = mid;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return low;
        }

        private bool _interestRateIsHigh(double price, double monthlyPayment, int loanTerm, double interestrate)
        {
            double balance = price;
            double interestAmount = 0;
            double monthlyInterestRate = interestrate/12;
            int term = loanTerm;
            while (term > 0)
            {
                interestAmount = balance*monthlyInterestRate/100;
                balance = balance + interestAmount - monthlyPayment;
                term--;
            }

            return (balance > 1e-12);
        }
    }
}
