
class Program
{
    static void Main()
    {

        Dictionary<string, dynamic>[] dicA;
        Dictionary<string, dynamic> dicB;

        dicA = new Dictionary<string, dynamic>[]
        {
            new Dictionary<string, dynamic>{
                { "string", 0  }
            },
            new Dictionary<string, dynamic>{
                { "string", 1  }
            },
            new Dictionary<string, dynamic>{
                { "string", 1  }
            },
            new Dictionary<string, dynamic>{
                { "string", 1  }
            },
            new Dictionary<string, dynamic>{
                { "string", 1  },
                { "pippo", 1  }
            }
        };

        dicB = new Dictionary<string, dynamic>
        {
                { "string", 1  },
                { "pippo", 1  },
        };

        var r = DifDic(dicA, dicB);
        foreach (var x in r)
            Console.WriteLine(x);
    }

    static List<string> DifDic(Dictionary<string, dynamic> DicA,
                              Dictionary<string, dynamic> DicB,
                              // Parametro opzionale che può essere null tramita ?.
                              List<string>? whiteList = null)
    {
        // Lista delle key dei valori differenti.
        List<string> difference = new();
        // Scorrimento del primo array associativo.
        foreach (var pairA in DicA)
        {
            /* Se c'è una lista dei valori da prendere in considerazione 
               vedi se la key è contenuta nella lista*/
            if (whiteList == null || whiteList.Contains(pairA.Key))
            {
                /* Aggiungi al risultato gli elementi del primo array che non si trovano nel secondo.
                   Anche se fossero altri array associativi non ci serve compararli quindi ritorna 
                   direttamente la key che viene inserita nel risultato*/
                if (!DicB.ContainsKey(pairA.Key))
                    difference.Add(pairA.Key);
                // Se la prima chiave è contenuta nel secondo array ed hanno valori diversi allora:
                else if (DicB.ContainsKey(pairA.Key) && !DicB[pairA.Key].Equals(pairA.Value))
                {
                    // Se il valore è un array associativo allora:
                    if (pairA.Value is Dictionary<string, dynamic>)
                    {
                        // Immissione dei risultati nella lista con l'aggiunta della key root.
                        foreach (var key in DifDic(pairA.Value, DicB[pairA.Key], whiteList))
                        {
                            difference.Add($"{pairA.Key}/{key}");
                        }
                    }
                    // Altrimenti aggiungi la key ai risultati.
                    else difference.Add(pairA.Key);
                }
            }
        }
        // Scorrimento del secondo array associativo.
        foreach (var pairB in DicB)
            /* Se c'è una lista dei valori da prendere in considerazione 
               vedi se la key è contenuta nella lista ed aggiungi al risultato 
               gli elementi del secondo array che non si trovano nel primo.*/
            if (!DicA.ContainsKey(pairB.Key) && (whiteList == null || whiteList.Contains(pairB.Key)))
                difference.Add(pairB.Key);

        return difference;
    }

    // Primo overraid con due array, di qualunque dimensione, di array associativi.
    static List<string> DifDic(Array ArrayDicA,
                              Array ArrayDicB,
                              // Parametro opzionale che può essere null tramita ?.
                              List<string>? whiteList = null)
    {
        // Lista delle key dei valori differenti.
        List<string> difference = new();
        // length è uguale alla lunghezza dell'array maggiore.
        var length = ArrayDicA.Length > ArrayDicB.Length ? ArrayDicA.Length : ArrayDicB.Length;

            for (int i = 0; i < length; i++)
            {
                try
                {
                /* Inserimento delle key nei risultati con l'aggiunta del numero
                   che indica la posizione nell'array */
                foreach (var key in DifDic(ArrayDicA.GetValue(i) as Dictionary<string, dynamic>,
                                            ArrayDicB.GetValue(i) as Dictionary<string, dynamic>, 
                                            whiteList))
                    difference.Add($"[{i}]{key}");
                }
                // Se un array ha più dictionary dell'altro.
                catch (IndexOutOfRangeException)
                {
                    // Aggiungi le chiavi dell'array nei risultati.
                    foreach (var key in (ArrayDicA.Length > ArrayDicB.Length ?
                                        ArrayDicA.GetValue(i) as Dictionary<string, dynamic> :
                                        ArrayDicB.GetValue(i) as Dictionary<string, dynamic>).Keys)
                        difference.Add($"[{i}]{key}");
                }
                //Cattura di tutte le eccezioni possibili.
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
            }

        return difference;
    }

    // Secondo overraid con un array, di qualunque dimensione, di array associativi
    // ed un array associativo.
    static List<string> DifDic(Array ArrayDicA,
                              Dictionary<string, dynamic> DicB,
                              // Parametro opzionale che può essere null tramita ?.
                              List<string>? whiteList = null)
    {
        // Lista delle key dei valori differenti.
        List<string> difference = new();

        for (int i = 0; i < ArrayDicA.Length; i++)
        {
            try
            {
                /* Inserimento delle key nei risultati con l'aggiunta del numero
                   che indica la posizione nell'array */
                foreach (var key in DifDic(ArrayDicA.GetValue(i) as Dictionary<string, dynamic>,
                                            DicB, whiteList))
                    difference.Add($"[{i}]{key}");
            }
            //Cattura di tutte le eccezioni possibili.
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        return difference;
    }

    // Terzo overraid con un array, di qualunque dimensione, di array associativi
    // ed un array associativo (inverso del secondo).
    static List<string> DifDic(Dictionary<string, dynamic> DicA,
                              Array ArrayDicB,
                              // Parametro opzionale che può essere null tramita ?.
                              List<string>? whiteList = null)
    {
        return DifDic(ArrayDicB, DicA, whiteList);
    }
}