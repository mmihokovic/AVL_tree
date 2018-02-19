using System.Windows.Forms;

namespace NASP
{
    public class Cvor
    {
      public Cvor(int vrijednost)
        {
            Vrijednost = vrijednost;
            FaktorRavnoteze = 0;
            Visina = 0;
        }

      public Cvor Roditelj { get; set; }
      public Cvor DesnoDijete { get; set; }
      public Cvor LijevoDijete { get; set; }
      public int FaktorRavnoteze { get; set; }
      public int Vrijednost { get; set; }
      public int Visina { get; set; }


      public TreeNode DohvatiCvorove()
        {
            var cvor = new TreeNode("Node value: " + Vrijednost +
                                " Balnce factor: " + FaktorRavnoteze);
            if (DesnoDijete != null)
            {
                cvor.Nodes.Add(DesnoDijete.DohvatiCvorove());
            }
            else
            {
                cvor.Nodes.Add("*empty*");
            }

            if (LijevoDijete != null)
            {
                cvor.Nodes.Add(LijevoDijete.DohvatiCvorove());
            }
            else
            {
                cvor.Nodes.Add("*empty*");
            }

            return cvor;
        }
    

           

    }
}
