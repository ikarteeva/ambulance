using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Скорая_помощь
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
 
        }

        Random rnd = new Random();


        double Equale(double a, double b)
        {
            double value = rnd.Next(0, 10);
            double x = a + ((value / 10) * (b - a));
            return x;
        }

        double Normal(int c, int sto)
        {
            double v = 1;
            double v1 = 6;
            double s = 4;
            while (s == 0 || s > 1 || s == 1)
            {
                v = (2 * rnd.NextDouble()) - 1;
                v1 = (2 * rnd.NextDouble()) - 1;
                s = (v * v) + (v1 * v1);
            }
            //double x = (1 / (sto * (Math.Sqrt(2 * Math.PI)))) * Math.Pow(Math.E, ((-(Math.Pow(value - c, 2))) / (2 * Math.Pow(sto, 2))));
            //double x = c + (Math.Cos(2 * Math.PI * value) * Math.Sqrt(-2 * Math.Log(value1))  * sto);
            double x = c + (sto * (v * Math.Sqrt((-2 * Math.Log(s)) / s)));
            return x;
        }
        int Diskret()
        {
            double value = rnd.Next(0, 100);
            if (value / 100 < 0.15)
            {
                return 5;
            }
            else if (value / 100 < 0.37)
            {
                return 8;
            }
            else if (value / 100 < 0.54)
            {
                return 12;
            }
            else if (value / 100 < 0.82)
            {
                return 15;
            }
            else
            {
                return 20;
            }
        }

        double Erlang(int k, double l)
        {
            double x = 1;
            for (int i = 0; i < k; i++)
            {
               double value = rnd.NextDouble();
                x = x * value;
            }

            x = -(1 / l) * Math.Log(x);

            return x;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView5.Rows.Clear();
            dataGridView6.Rows.Clear();

            Form1 func = new Form1();
            int a = Convert.ToInt32(textBox7.Text);
            int b = Convert.ToInt32(textBox8.Text);
            int c = Convert.ToInt32(textBox5.Text);
            int sto = Convert.ToInt32(textBox6.Text);
            double l = Convert.ToDouble(textBox3.Text);
            int k = 3;

            #region ввод переменных

            List <Model> ListCanal= new List<Model>();

            int colcanal = Convert.ToInt32(textBox1.Text);

            List<Canal> ListCanalClass = new List<Canal>();

            for (int i = 0; i < colcanal; i++)
            {
                ListCanalClass.Add(new Canal() { nomercanala = i, timecall = DateTime.Today });
            }


            int colauto = Convert.ToInt32(textBox2.Text); 

            List<DateTime> auto = new List<DateTime>();

            for (int i = 0; i < colauto; i++)
            {
                DateTime auto1 = DateTime.Today;
                auto.Add(auto1);
            }

            #endregion

            #region имитация


            for (int i = 1; i < 6; i++)
            {
                DateTime date1 = DateTime.Today;
                DateTime flag = date1.AddDays(1);
                DateTime perehod = date1;


                while (date1 < flag)
                {
                    double s = func.Erlang(k, l);

                    DateTime calltime = perehod.AddMinutes(s).AddSeconds(Convert.ToInt32(textBox5.Text));

                    if (calltime > flag)
                    {
                        break;
                    }

                    Model model1 = new Model();
                    model1.totaltime = null;

                    model1.timecall = calltime;

                    while (model1.totaltime == null)
                    {
                        model1.day = i;

                        int canalchange = 0;
                        int oppa = 0;
                        int vhozhdenie = 0;

                        for(int m=0; m<ListCanalClass.LongCount(); m++)
                        {
                                if (calltime > ListCanalClass[m].timecall)
                                {

                                vhozhdenie++;

                                    model1.canal = m+1;

                                    model1.timereceprion = calltime;

                                    canalchange = m;

                                    int schet = 1;
                                    DateTime callplusexit = new DateTime();
                                    DateTime min = auto.First();
                                    int indexmin = 0;

                                    foreach (var au in auto)
                                    {
                                        if (ListCanalClass[m].timecall > au)
                                        {
                                            model1.numberauto = schet;
                                            int y = func.Diskret();
                                            model1.distancetopacient = y; //расстояние до пациента
                                            double x = func.Equale(a, b); //скорость автомобиля
                                            double z = func.Normal(c, sto); //время оказания помощи больному
                                            model1.timehelp = Math.Round(z);

                                            double minutetime = Math.Round((((2 * y) / x) * 60) + z);

                                            model1.timeexit = minutetime;

                                            callplusexit = ListCanalClass[m].timecall.AddMinutes(minutetime);

                                            model1.totaltime = (callplusexit - model1.timecall);

                                            ListCanal.Add(model1);

                                            break;
                                        }

                                        else
                                        {
                                            if (au < min)
                                            {
                                                min = au;
                                                indexmin = schet - 1;
                                            }
                                        }

                                        schet++;
                                    }


                                    if (schet == auto.LongCount() + 1)
                                    {
                                        model1.numberauto = indexmin + 1;
                                        int y = func.Diskret();
                                        model1.distancetopacient = y; //расстояние до пациента
                                        double x = func.Equale(a, b); //скорость автомобиля
                                        double z = func.Normal(c, sto); //время оказания помощи больному
                                        model1.timehelp = Math.Round(z);

                                        double minutetime = Math.Round((((2 * y) / x) * 60) + z);

                                        model1.timeexit = minutetime;

                                        callplusexit = min.AddMinutes(minutetime);

                                        model1.totaltime = (callplusexit - model1.timecall);

                                        auto[indexmin] = callplusexit;

                                        ListCanal.Add(model1);

                                    }
                                    else
                                    {
                                        auto[schet - 1] = callplusexit;
                                    }

                                oppa = m;
                                break;

                            }
                        }

                        if (vhozhdenie == 0)
                        {
                            calltime = calltime.AddSeconds(20);
                        }

                        else
                        {
                            ListCanalClass[oppa].timecall = calltime.AddMinutes(Convert.ToInt32(textBox9.Text));
                        }
                                 
                    }

                    date1 = model1.timereceprion.AddMinutes(Convert.ToInt32(textBox9.Text));
                    perehod = model1.timecall;

                }
            }

            #endregion 


            int toe1 = 0;
            int toe2 = 0;
            int toe3 = 0;
            int toe4 = 0;
            int toe5 = 0;

            int count = 0;
            TimeSpan ? srednee = new TimeSpan();

            List<int> probeg = new List<int>();

            int zapolnenie = 0;

            while (zapolnenie < auto.LongCount())
            {
                probeg.Add(0);
                zapolnenie++;
            }

           


            #region вывод

            int j = 0;


                foreach (var mod in ListCanal.OrderBy(u => u.timecall).Distinct())
                {
                    count++;
                    srednee = srednee + mod.totaltime;

                    for (int i = 0; i < auto.LongCount(); i++)
                    {
                        if (i + 1 == mod.numberauto)
                        {
                            probeg[i] = probeg[i] + (mod.distancetopacient * 2);
                        }
                    }


                    if (mod.day == 1)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[toe1].Cells[0].Value = mod.timecall.ToString("HH:mm:ss");
                        dataGridView1.Rows[toe1].Cells[1].Value = (mod.timereceprion - mod.timecall).Minutes.ToString();
                        dataGridView1.Rows[toe1].Cells[2].Value = mod.timereceprion.ToString("HH:mm:ss");
                        dataGridView1.Rows[toe1].Cells[3].Value = mod.canal;
                        dataGridView1.Rows[toe1].Cells[4].Value = mod.numberauto;
                        dataGridView1.Rows[toe1].Cells[5].Value = mod.distancetopacient;
                        dataGridView1.Rows[toe1].Cells[6].Value = mod.timehelp;
                        dataGridView1.Rows[toe1].Cells[7].Value = mod.timeexit;
                        dataGridView1.Rows[toe1].Cells[8].Value = mod.totaltime.Value.Days + " дней " + mod.totaltime.Value.Hours + " часов " + mod.totaltime.Value.Minutes + " минут";
                        toe1++;
                    }
                    else if (mod.day == 2)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[toe2].Cells[0].Value = mod.timecall.ToString("HH:mm:ss");
                        dataGridView2.Rows[toe2].Cells[1].Value = (mod.timereceprion - mod.timecall).Minutes.ToString();
                        dataGridView2.Rows[toe2].Cells[2].Value = mod.timereceprion.ToString("HH:mm:ss");
                        dataGridView2.Rows[toe2].Cells[3].Value = mod.canal;
                        dataGridView2.Rows[toe2].Cells[4].Value = mod.numberauto;
                        dataGridView2.Rows[toe2].Cells[5].Value = mod.distancetopacient;
                        dataGridView2.Rows[toe2].Cells[6].Value = mod.timehelp;
                        dataGridView2.Rows[toe2].Cells[7].Value = mod.timeexit;
                        dataGridView2.Rows[toe2].Cells[8].Value = mod.totaltime.Value.Days + " дней " + mod.totaltime.Value.Hours + " часов " + mod.totaltime.Value.Minutes + " минут";
                        toe2++;
                    }
                    else if (mod.day == 3)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[toe3].Cells[0].Value = mod.timecall.ToString("HH:mm:ss");
                        dataGridView3.Rows[toe3].Cells[1].Value = (mod.timereceprion - mod.timecall).Minutes.ToString();
                        dataGridView3.Rows[toe3].Cells[2].Value = mod.timereceprion.ToString("HH:mm:ss");
                        dataGridView3.Rows[toe3].Cells[3].Value = mod.canal;
                        dataGridView3.Rows[toe3].Cells[4].Value = mod.numberauto;
                        dataGridView3.Rows[toe3].Cells[5].Value = mod.distancetopacient;
                        dataGridView3.Rows[toe3].Cells[6].Value = mod.timehelp;
                        dataGridView3.Rows[toe3].Cells[7].Value = mod.timeexit;
                        dataGridView3.Rows[toe3].Cells[8].Value = mod.totaltime.Value.Days + " дней " + mod.totaltime.Value.Hours + " часов " + mod.totaltime.Value.Minutes + " минут";
                        toe3++;
                    }
                    else if (mod.day == 4)
                    {
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[toe4].Cells[0].Value = mod.timecall.ToString("HH:mm:ss");
                        dataGridView4.Rows[toe4].Cells[1].Value = (mod.timereceprion - mod.timecall).Minutes.ToString();
                        dataGridView4.Rows[toe4].Cells[2].Value = mod.timereceprion.ToString("HH:mm:ss");
                        dataGridView4.Rows[toe4].Cells[3].Value = mod.canal;
                        dataGridView4.Rows[toe4].Cells[4].Value = mod.numberauto;
                        dataGridView4.Rows[toe4].Cells[5].Value = mod.distancetopacient;
                        dataGridView4.Rows[toe4].Cells[6].Value = mod.timehelp;
                        dataGridView4.Rows[toe4].Cells[7].Value = mod.timeexit;
                        dataGridView4.Rows[toe4].Cells[8].Value = mod.totaltime.Value.Days + " дней " + mod.totaltime.Value.Hours + " часов " + mod.totaltime.Value.Minutes + " минут";
                        toe4++;
                    }
                    else if (mod.day == 5)
                    {
                        dataGridView5.Rows.Add();
                        dataGridView5.Rows[toe5].Cells[0].Value = mod.timecall.ToString("HH:mm:ss");
                        dataGridView5.Rows[toe5].Cells[1].Value = (mod.timereceprion - mod.timecall).Minutes.ToString();
                        dataGridView5.Rows[toe5].Cells[2].Value = mod.timereceprion.ToString("HH:mm:ss");
                        dataGridView5.Rows[toe5].Cells[3].Value = mod.canal;
                        dataGridView5.Rows[toe5].Cells[4].Value = mod.numberauto;
                        dataGridView5.Rows[toe5].Cells[5].Value = mod.distancetopacient;
                        dataGridView5.Rows[toe5].Cells[6].Value = mod.timehelp;
                        dataGridView5.Rows[toe5].Cells[7].Value = mod.timeexit;
                        dataGridView5.Rows[toe5].Cells[8].Value = mod.totaltime.Value.Days + " дней " + mod.totaltime.Value.Hours + " часов " + mod.totaltime.Value.Minutes + " минут";
                        toe5++;
                    }
                }

            #endregion


            var avg = TimeSpan.FromMilliseconds(srednee.Value.TotalMilliseconds / count);


            label3.Text = avg.Days + " дней " + avg.Hours + " часов " + avg.Minutes + " минут";

            label6.Text = count.ToString();

            for (int i = 0; i < auto.LongCount(); i++)
            {
                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells[0].Value = i + 1;
                dataGridView6.Rows[i].Cells[1].Value = probeg[i];
            }

        }
    }
}
