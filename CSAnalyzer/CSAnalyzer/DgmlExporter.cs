using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSAnalyzer
{
    public class DgmlExporter
    {
        private readonly List<string> _nodes = new List<string>();
        private readonly List<(string Source, string Target)> _edges = new List<(string Source, string Target)>();

        /// <summary>
        /// Adiciona um nó ao grafo.
        /// </summary>
        public void AddNode(string id)
        {
            if (!_nodes.Contains(id))
            {
                _nodes.Add(id);
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo.
        /// </summary>
        public void AddEdge(string source, string target)
        {
            _edges.Add((source, target));
        }

        /// <summary>
        /// Exporta o grafo para um arquivo DGML.
        /// </summary>
        public void Export(string filePath)
        {
            // Gera o grafo DGML
            var xDoc = new XDocument(
                new XElement("DirectedGraph",
                    new XAttribute(XNamespace.Xmlns + "dgml", "http://schemas.microsoft.com/vs/2009/dgml"), // ToDo: Corrigir erro dgml
                    new XElement("Nodes",
                        _nodes.Select(node => new XElement("Node", new XAttribute("Id", node)))
                    ),
                    new XElement("Links",
                        _edges.Select(edge => new XElement("Link",
                            new XAttribute("Source", edge.Source),
                            new XAttribute("Target", edge.Target)))
                    )
                )
            );

            xDoc.Save(filePath);
        }
    }
}
