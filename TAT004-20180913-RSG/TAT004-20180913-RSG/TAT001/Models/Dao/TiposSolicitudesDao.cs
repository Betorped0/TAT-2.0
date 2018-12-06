using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Models.Dao
{
    public class TiposSolicitudesDao
    {
        readonly TAT001Entities db = new TAT001Entities();
        public List<TSOLT> ListaTiposSolicitudes(string spras_id, string id, bool? esReversa = false)
        {
            return db.TSOLs
                .Where(x => ((esReversa.Value && x.TSOLR == null) || !esReversa.Value) && (id == null || x.ID == id) && (x.ACTIVO != null && x.ACTIVO.Value))
                .Join(db.TSOLTs, s => s.ID, st => st.TSOL_ID, (s, st) => st)
                .Where(x => x.SPRAS_ID == spras_id).ToList();
        }
        public  List<SelectListItem> ComboTiposSolicitudes( string spras_id, string id, bool? esReversa = false)
        {
            return ListaTiposSolicitudes(spras_id,id,esReversa)
                .Select(x => new SelectListItem
                {
                    Value = x.TSOL_ID,
                    Text = (x.TSOL_ID + " - " + x.TXT50)
                }).ToList();
        }

        public List<SelectTreeItem> TreePadresTiposSolicitudes(string spras_id)
        {

            List<SelectTreeItem> tree = new List<SelectTreeItem>();
                db.TSOL_GROUP
                    .Where(x =>x.ID_PADRE == null && x.TIPO_PADRE == null)
                    .Join(db.TSOL_GROUPT, tg => new { tg.ID, tg.TIPO }, tgt => new { ID = tgt.TSOL_GROUP_ID, TIPO = tgt.TSOL_GROUP_TIPO }, (tg, tgt) => tgt)
                    .Where(x => x.SPRAS_ID == spras_id).OrderBy(x => x.TSOL_GROUP_ID)
                    .ToList().ForEach(x =>
                    {
                        tree.Add(new SelectTreeItem
                        {
                            text = x.TXT50,
                            expanded = false,
                            items = ObtenerItemsSelectTree(x.TSOL_GROUP_ID, x.TSOL_GROUP_TIPO, spras_id,true),
                            value= x.TSOL_GROUP_ID+"|" + x.TSOL_GROUP_TIPO
                        });
                    });

            tree.Add(new SelectTreeItem
            {
                text = "Reverso",
                value = "REV",
                expanded = false
            });

            return tree;
        }
        public  List<SelectTreeItem> TreeTiposSolicitudes(string sociedad_id, string spras_id, string tipo = null, bool? esReversa = false)
        {
            // tipo
            // SD = Solicitud directa
            // SR = Solicitud relacionada

            List<SelectTreeItem> tree = new List<SelectTreeItem>();
            if (esReversa.Value)
            {
                tree.Add(new SelectTreeItem
                {
                    text = "Reverso",
                    expanded = false,
                    items = ObtenerItemsTSOLT( "", "", spras_id, esReversa)
                });
            }
            else
            {
                db.TSOL_GROUP
                    .Where(x =>
                    x.ID_PADRE == null && x.TIPO_PADRE == null
                    && (tipo == x.TIPO || tipo == null)
                    && (((sociedad_id == "KCMX" || sociedad_id == "KLCO") && x.ID != "1_5_OP" && x.ID != "2_5_OP") || (sociedad_id != "KCMX" && sociedad_id != "KLCO")))
                    .Join(db.TSOL_GROUPT, tg => new { tg.ID,  tg.TIPO }, tgt => new { ID = tgt.TSOL_GROUP_ID, TIPO = tgt.TSOL_GROUP_TIPO }, (tg, tgt) => tgt)
                    .Where(x => x.SPRAS_ID == spras_id).OrderBy(x => x.TSOL_GROUP_ID)
                    .ToList().ForEach(x =>
                    {
                        SelectTreeItem item = new SelectTreeItem
                        {
                            text = x.TXT50,
                            expanded = false,
                            items = ObtenerItemsSelectTree( x.TSOL_GROUP_ID, x.TSOL_GROUP_TIPO, spras_id)
                        };
                        if (item.items.Any())
                        {
                            tree.Add(item);
                        }
                    });
            }
            return tree;
        }

        List<SelectTreeItem> ObtenerItemsSelectTree(string id_padre, string tipo_padre, string spras_id,bool soloNodoSup=false)
        {
            List<SelectTreeItem> items = new List<SelectTreeItem>();
            db.TSOL_GROUP
                .Where(x => x.ID_PADRE == id_padre && x.TIPO_PADRE == tipo_padre)
                .ToList().ForEach(x =>
                {
                    SelectTreeItem item = new SelectTreeItem
                    {
                        text = x.DESCRIPCION,
                        expanded = false,
                        items = ObtenerItemsSelectTree(x.ID, x.TIPO, spras_id, soloNodoSup)
                    };
                    if (!item.items.Any() && !soloNodoSup)
                    {
                        item.items = ObtenerItemsTSOLT( x.ID, x.TIPO, spras_id);
                    }
                    else if (!item.items.Any() && soloNodoSup)
                    {
                        item.value = x.ID + "|" + x.TIPO;
                    }
                    items.Add(item);
                });
            if (!items.Any() && !soloNodoSup)
            {
                items = ObtenerItemsTSOLT( id_padre, tipo_padre, spras_id);
            }
            return items;
        }
       public List<SelectTreeItem> ObtenerItemsTSOLT(string id_padre, string tipo_padre, string spras_id, bool? esReversa = false)
        {
            List<SelectTreeItem> items = new List<SelectTreeItem>();
            if (esReversa.Value)
            {
                items = db.TSOLs
                    .Where(x => x.TSOLR == null && (x.ACTIVO != null && x.ACTIVO.Value))
                    .Join(db.TSOLTs, s => s.ID, st => st.TSOL_ID, (s, st) => st)
                    .Where(x => x.SPRAS_ID == spras_id)
                    .Select(x => new SelectTreeItem
                    {
                        value = x.TSOL_ID,
                        text = (x.TSOL_ID + " - " + x.TXT50),
                        expanded = true
                    })
                    .ToList();
            }
            else
            {
                db.TSOL_TREE
                           .Where(y => y.TSOL_GROUP_ID == id_padre && y.TSOL_GROUP_TIPO == tipo_padre)
                           .Join(db.TSOLTs, tst => tst.TSOL_ID, st => st.TSOL_ID, (tst, st) => st)
                           .Where(y => y.SPRAS_ID == spras_id && (y.TSOL.ACTIVO != null && y.TSOL.ACTIVO.Value))
                           .ToList().ForEach(y =>
                           {
                               items.Add(new SelectTreeItem
                               {
                                   value = y.TSOL_ID,
                                   text = (y.TSOL_ID + " - " + y.TXT50),
                                   expanded = true
                               });
                           });
            }
            return items;
        }
    }
}