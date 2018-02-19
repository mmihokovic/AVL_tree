using System;
using System.Windows.Forms;

namespace NASP
{
    public class AvlStablo
    {
        private Cvor _korijen;
        public AvlStablo()
        {
            _korijen = null;
        }

        /// <summary>
        /// Dodaje čvor u stablo.
        /// </summary>
        /// <param name="vrijednost"></param>
        public void Dodaj(int vrijednost)
        {
            Cvor slobodanCvor = null;
            var privremeniCvor = _korijen;

            // ako stablo nema elemenata
            if (_korijen == null)
            {
                _korijen = new Cvor(vrijednost);
            }

            else
            {
                // pronađi slobodno mjesto za novi čvor
                while (privremeniCvor != null)
                {
                    slobodanCvor = privremeniCvor;
                    if (vrijednost < privremeniCvor.Vrijednost)
                    {
                        privremeniCvor = privremeniCvor.LijevoDijete;
                    }

                    else
                    {
                        privremeniCvor = privremeniCvor.DesnoDijete;
                    }
                }

                // ubaci čvor na slobodno mjesto
                if (vrijednost < slobodanCvor.Vrijednost)
                {
                    slobodanCvor.LijevoDijete = new Cvor(vrijednost);
                    slobodanCvor.LijevoDijete.Roditelj = slobodanCvor;
                    // podesi faktore ravnoteže i uravnoteži stablo
                    OsvijeziFaktoreDodavanje(slobodanCvor.LijevoDijete);

                }
                else
                {
                    slobodanCvor.DesnoDijete = new Cvor(vrijednost);
                    slobodanCvor.DesnoDijete.Roditelj = slobodanCvor;
                    // podesi faktore ravnoteže i uravnoteži stablo
                    OsvijeziFaktoreDodavanje(slobodanCvor.DesnoDijete);
                }
            }
        }

        #region balansiranje stabla

        private void Balansiraj(Cvor trenutniCvor)
        {
          if (trenutniCvor.Roditelj == null)
                return;
          if (Math.Abs(trenutniCvor.Roditelj.FaktorRavnoteze) == 2)
          {
            if (trenutniCvor.Roditelj.FaktorRavnoteze == -2)
            {
              if (trenutniCvor.FaktorRavnoteze == -1)
                DesnaRotacija(trenutniCvor.Roditelj);
              else
                LijevaDesnaRotacija(trenutniCvor);
            }
            else
            {
              if (trenutniCvor.FaktorRavnoteze == 1)
                LijevaRotacija(trenutniCvor.Roditelj);
              else
                DesnaLijevaRotacija(trenutniCvor);
            }
          }
        }

        private void BalansirajBrisanje(Cvor trenutniCvor)
        {
          if (trenutniCvor == null)
                return;
          if (Math.Abs(trenutniCvor.FaktorRavnoteze) == 2)
          {
            if (trenutniCvor.FaktorRavnoteze == -2)
            {
              if (trenutniCvor.LijevoDijete != null)
              {
                if (trenutniCvor.LijevoDijete.FaktorRavnoteze == -1 ||
                    trenutniCvor.LijevoDijete.FaktorRavnoteze == 0)
                {
                  DesnaRotacija(trenutniCvor);
                  return;
                }

                if (trenutniCvor.LijevoDijete.FaktorRavnoteze == 1)
                {
                  LijevaDesnaRotacija(trenutniCvor.LijevoDijete);
                  return;
                }
              }
              if (trenutniCvor.DesnoDijete != null)
              {
                if (trenutniCvor.DesnoDijete.FaktorRavnoteze == -1 ||
                    trenutniCvor.DesnoDijete.FaktorRavnoteze == 0)
                {
                  DesnaRotacija(trenutniCvor);
                }
                else if (trenutniCvor.DesnoDijete.FaktorRavnoteze == 1)
                {
                  LijevaDesnaRotacija(trenutniCvor.DesnoDijete);
                }
              }
            }

            else if (trenutniCvor.FaktorRavnoteze == 2)
            {
              if (trenutniCvor.LijevoDijete != null)
              {
                if (trenutniCvor.LijevoDijete.FaktorRavnoteze == -1
                    || trenutniCvor.LijevoDijete.FaktorRavnoteze == 0)
                {
                  LijevaRotacija(trenutniCvor);
                  return;
                }
                if (trenutniCvor.LijevoDijete.FaktorRavnoteze == 1)
                {
                  DesnaLijevaRotacija(trenutniCvor.LijevoDijete);
                  return;
                }
              }
              if (trenutniCvor.DesnoDijete != null)
              {
                if (trenutniCvor.DesnoDijete.FaktorRavnoteze == -1
                    || trenutniCvor.DesnoDijete.FaktorRavnoteze == 0)
                {
                  DesnaLijevaRotacija(trenutniCvor.DesnoDijete);
                }
                else if (trenutniCvor.DesnoDijete.FaktorRavnoteze == 1)
                {
                  LijevaRotacija(trenutniCvor);
                }
              }
            }
          }
        }
        
        #endregion


        #region rotacije
        /// <summary>
        /// Desna rotacija, zamijena čvora u smijeru kazaljke na satu.
        /// </summary>
        /// <param name="trenutniCvor"></param>
        private void DesnaRotacija(Cvor trenutniCvor)
        {
            if (trenutniCvor.LijevoDijete == null)
                throw new Exception("Right rotation: error");

            var dijeteCvora = trenutniCvor.LijevoDijete;
            trenutniCvor.LijevoDijete = dijeteCvora.DesnoDijete;

            if (dijeteCvora.DesnoDijete != null)
            {
                dijeteCvora.DesnoDijete.Roditelj = trenutniCvor;
            }

            dijeteCvora.Roditelj = trenutniCvor.Roditelj;

            if (trenutniCvor.Roditelj == null)
            {
                _korijen = dijeteCvora;
            }
            else if (trenutniCvor == trenutniCvor.Roditelj.DesnoDijete)
            {
                trenutniCvor.Roditelj.DesnoDijete = dijeteCvora;
            }
            else
            {
                trenutniCvor.Roditelj.LijevoDijete = dijeteCvora;
            }

            dijeteCvora.DesnoDijete = trenutniCvor;
            trenutniCvor.Roditelj = dijeteCvora;

            ObnoviFaktore(trenutniCvor);
        }

        /// <summary>
        /// Lijeva rotacija, zamijena čvora suprotno kazaljci na satu.
        /// </summary>
        /// <param name="trenutniCvor"></param>
        private void LijevaRotacija(Cvor trenutniCvor)
        {
            if (trenutniCvor.DesnoDijete == null)
                throw new Exception("Left rotation: error");

            var dijeteCvora = trenutniCvor.DesnoDijete;
            trenutniCvor.DesnoDijete = dijeteCvora.LijevoDijete;

            if (dijeteCvora.LijevoDijete != null)
                dijeteCvora.LijevoDijete.Roditelj = trenutniCvor;

            dijeteCvora.Roditelj = trenutniCvor.Roditelj;

            if (trenutniCvor.Roditelj == null)
                _korijen = dijeteCvora;
            else if (trenutniCvor == trenutniCvor.Roditelj.LijevoDijete)
                trenutniCvor.Roditelj.LijevoDijete = dijeteCvora;
            else
                trenutniCvor.Roditelj.DesnoDijete = dijeteCvora;

            dijeteCvora.LijevoDijete = trenutniCvor;
            trenutniCvor.Roditelj = dijeteCvora;

            ObnoviFaktore(trenutniCvor);
        }

        private void DesnaLijevaRotacija(Cvor trenutniCvor)
        {
            DesnaRotacija(trenutniCvor);
            LijevaRotacija(trenutniCvor.Roditelj.Roditelj);
        }

        private void LijevaDesnaRotacija(Cvor trenutniCvor)
        {
            LijevaRotacija(trenutniCvor);
            DesnaRotacija(trenutniCvor.Roditelj.Roditelj);
        }
        
        #endregion

        #region obnavljanje faktora ravnoteže
        /// <summary>
        /// Obnavlja faktore ravnoteže nakon rotacije
        /// </summary>
        /// <param name="cvor"></param>
        private void ObnoviFaktore(Cvor cvor)
        {
            var lijevaVisina = 0;
            var desnaVisina = 0;

            if (cvor.LijevoDijete != null)
            {
                lijevaVisina = cvor.LijevoDijete.Visina + 1;
            }

            if (cvor.DesnoDijete != null)
            {
                desnaVisina = cvor.DesnoDijete.Visina + 1;
            }

            cvor.Visina = Math.Max(desnaVisina, lijevaVisina);
            cvor.FaktorRavnoteze = desnaVisina - lijevaVisina;

            cvor = cvor.Roditelj;
            if (cvor == null) { return; }

            lijevaVisina = 0;
            desnaVisina = 0;

            if (cvor.LijevoDijete != null)
            {
                lijevaVisina = cvor.LijevoDijete.Visina + 1;
            }

            if (cvor.DesnoDijete != null)
            {
                desnaVisina = cvor.DesnoDijete.Visina + 1;
            }

            cvor.Visina = Math.Max(desnaVisina, lijevaVisina);
            cvor.FaktorRavnoteze = desnaVisina - lijevaVisina;

        }



        /// <summary>
        /// Obnavlja faktore ravnoteže nakon dodavanja novog čvora.
        /// Istovremeno obnavlja faktore ravnoteže trenutnog čvora i njegovog roditelja.
        /// U slučaju da je stablo izvan ravnoteže, vrši balansiranje stabla.
        /// </summary>
        /// <param name="trenutniCvor"></param>
        private void OsvijeziFaktoreDodavanje(Cvor trenutniCvor)
        {
            if (trenutniCvor == null)
            {
                return;
            }

            trenutniCvor.Visina = 0;

            while (trenutniCvor.Roditelj != null)
            {
                var lijevaVisinaRoditelja = 0;
                var desnaVisinaRoditelja = 0;

                if (trenutniCvor.Roditelj.LijevoDijete != null)
                {
                    lijevaVisinaRoditelja = trenutniCvor.Roditelj.LijevoDijete.Visina + 1;
                }

                if (trenutniCvor.Roditelj.DesnoDijete != null)
                {
                    desnaVisinaRoditelja = trenutniCvor.Roditelj.DesnoDijete.Visina + 1;
                }

                trenutniCvor.Roditelj.Visina = Math.Max(lijevaVisinaRoditelja, desnaVisinaRoditelja);

                var lijevaVisina = 0;
                var desnaVisina = 0;
                if (trenutniCvor.LijevoDijete != null)
                {
                    lijevaVisina = trenutniCvor.LijevoDijete.Visina + 1;
                }
                if (trenutniCvor.DesnoDijete != null)
                {
                    desnaVisina = trenutniCvor.DesnoDijete.Visina + 1;
                }
                trenutniCvor.Visina = Math.Max(lijevaVisina, desnaVisina);
                trenutniCvor.FaktorRavnoteze = desnaVisina - lijevaVisina;
                trenutniCvor.Roditelj.FaktorRavnoteze = desnaVisinaRoditelja - lijevaVisinaRoditelja;

                Balansiraj(trenutniCvor);

                if (desnaVisinaRoditelja == lijevaVisinaRoditelja)
                {
                    break;
                }

                trenutniCvor = trenutniCvor.Roditelj;
                if (trenutniCvor == null)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Obnavlja faktore ravnoteže nakon brisanja čvora.
        /// </summary>
        /// <param name="trenutniCvor"></param>
        private void OsvijeziFaktoreBrisanje(Cvor trenutniCvor)
        {
            if (trenutniCvor == null)
            {
                return;
            }

            trenutniCvor.Visina = 0;
            while (trenutniCvor != null)
            {
                var lijevaVisina = 0;
                var desnaVisina = 0;

                if (trenutniCvor.LijevoDijete != null)
                {
                    lijevaVisina = trenutniCvor.LijevoDijete.Visina + 1;
                }
                if (trenutniCvor.DesnoDijete != null)
                {
                    desnaVisina = trenutniCvor.DesnoDijete.Visina + 1;
                }

                trenutniCvor.Visina = Math.Max(lijevaVisina, desnaVisina);
                trenutniCvor.FaktorRavnoteze = desnaVisina - lijevaVisina;

                if (Math.Abs(trenutniCvor.FaktorRavnoteze) == 1)
                {
                    break;
                }
              if (Math.Abs(trenutniCvor.FaktorRavnoteze) == 2)
              {
                BalansirajBrisanje(trenutniCvor);
                if (Math.Abs(trenutniCvor.FaktorRavnoteze) == 1)
                {
                  break;
                }
              }

              trenutniCvor = trenutniCvor.Roditelj;
            }
        } 
        #endregion
        
        /// <summary>
        /// Briše čvor iz stabla sa zadanom vrijednošću.
        /// </summary>
        /// <param name="vrijednost"></param>
        public void BrisiCvor(int vrijednost)
        {
            var brisaniCvor = PronadiCvor(vrijednost);
            if (brisaniCvor == null)
            {
                return;
            }

                // ako nema dijece, znači da je zadnji
                if (brisaniCvor.LijevoDijete == null && brisaniCvor.DesnoDijete == null)
                {
                    var roditeljBrisanog = brisaniCvor.Roditelj;
                    // ukloni čvor
                    PromijeniDijeteRoditelja(brisaniCvor.Roditelj, brisaniCvor, null);
                    OsvijeziFaktoreBrisanje(roditeljBrisanog);
                }
                
                // ako ima samo jedno dijete 
                //  ^ == EXOR
                else if (brisaniCvor.LijevoDijete == null ^ brisaniCvor.DesnoDijete == null)
                {
                    var roditeljBrisanog = brisaniCvor.Roditelj;
                    if (brisaniCvor.LijevoDijete != null)
                    {
                        // ukloni čvor
                        PromijeniDijeteRoditelja(brisaniCvor.Roditelj, brisaniCvor, brisaniCvor.LijevoDijete);
                        OsvijeziFaktoreBrisanje(roditeljBrisanog);
                    }
                    else
                    {
                        // ukloni čvor
                        PromijeniDijeteRoditelja(brisaniCvor.Roditelj, brisaniCvor, brisaniCvor.DesnoDijete);
                        OsvijeziFaktoreBrisanje(roditeljBrisanog);
                    }
                }
                    
                // ako ima oba dijeteta
                else
                {
                    // pronađi najveći manji od brisanog čvora
                    var najveciManji = brisaniCvor.LijevoDijete;
                    while (najveciManji.DesnoDijete != null)
                    {
                        najveciManji = najveciManji.DesnoDijete;
                    }

                    // kopiraj podatke iz najvećeg u brisani
                    brisaniCvor.Vrijednost = najveciManji.Vrijednost; 

                    // prebaci lijevo dijete najmanjeg na mjesto brisani.lijevi.desni
                    brisaniCvor.LijevoDijete.DesnoDijete = najveciManji.LijevoDijete;
                    if (brisaniCvor.LijevoDijete.DesnoDijete != null)
                    {
                        brisaniCvor.LijevoDijete.DesnoDijete.Roditelj = brisaniCvor.LijevoDijete;
                    }
                    // ukloni čvor največeg
                    var roditeljNajveceg = najveciManji.Roditelj;
                    PromijeniDijeteRoditelja(najveciManji.Roditelj, najveciManji, null);

                    OsvijeziFaktoreBrisanje(roditeljNajveceg);
                }
            
        }

        #region pomoćne funkcije
        /// <summary>
        /// Traži čvor unutar stabla koji sadrži zadanu vrijednost.
        /// </summary>
        /// <param name="vrijednost"></param>
        /// <returns>Vraća prvi pronađeni čvor.</returns>
        private Cvor PronadiCvor(int vrijednost)
        {
            var privremeniCvor = _korijen;
            while (privremeniCvor != null)
            {
                if (vrijednost < privremeniCvor.Vrijednost)
                {
                    privremeniCvor = privremeniCvor.LijevoDijete;
                }
                else if (vrijednost > privremeniCvor.Vrijednost)
                {
                    privremeniCvor = privremeniCvor.DesnoDijete;
                }
                else if (vrijednost == privremeniCvor.Vrijednost)
                {
                    return privremeniCvor;
                }
            }
            return null;
        }

    /// <summary>
    /// Zamijenjuje čvor s novim čvorom.
    /// </summary>
    /// <param name="roditelj">Čvor iznad čvora trenutniCvor.</param>
    /// <param name="trenutniCvor">Čvor na koji želimo smjestiti novi čvor.</param>
    /// <param name="noviCvor">Novi čvor koji želimo umetnuti na mjesto trenutniCvor.</param>
    private void PromijeniDijeteRoditelja(Cvor roditelj, Cvor trenutniCvor, Cvor noviCvor)
        {
            if (roditelj == null)
            {
                if (_korijen == trenutniCvor)
                {
                    _korijen = noviCvor;
                    if (noviCvor != null)
                    {
                        _korijen.Roditelj = null;
                    }
                }
                return;
            }

            if (roditelj.LijevoDijete == trenutniCvor)
            {
                roditelj.LijevoDijete = noviCvor;
                if (noviCvor != null)
                {
                    roditelj.LijevoDijete.Roditelj = roditelj;
                }
            }
            else
            {
                roditelj.DesnoDijete = noviCvor;
                if (noviCvor != null)
                {
                    roditelj.DesnoDijete.Roditelj = roditelj;
                }
            }

        }

        /// <summary>
        /// Obilazi stablo i kopira čvorove iz AVL stabla u stablo za prikaz u grafičkom sučelju.
        /// </summary>
        /// <returns></returns>
        public TreeNode DohvatiCvorove()
        {
            if (_korijen == null)
            {
                return new TreeNode();
            }
            var cvor = new TreeNode("Node value: " + _korijen.Vrijednost +
                                " Balance factor: " + _korijen.FaktorRavnoteze);
            if (_korijen.DesnoDijete != null)
            {
                cvor.Nodes.Add(_korijen.DesnoDijete.DohvatiCvorove());
            }
            else
            {
                cvor.Nodes.Add("*empty*");
            }

            if (_korijen.LijevoDijete != null)
            {
                cvor.Nodes.Add(_korijen.LijevoDijete.DohvatiCvorove());
            }
            else
            {
                cvor.Nodes.Add("*empty*");
            }

            return cvor;
        }
        
        #endregion

    }
}
