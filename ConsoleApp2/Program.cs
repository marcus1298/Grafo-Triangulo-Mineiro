using System;
using System.Collections.Generic;

public class DijkstraAlgorithm
{
    private Dictionary<string, Dictionary<string, int>> graph;

    public DijkstraAlgorithm(Dictionary<string, Dictionary<string, int>> graph)
    {
        this.graph = graph;
    }

    public List<string> FindShortestPath(string start, string end)
    {
        var distances = new Dictionary<string, int>();
        var previous = new Dictionary<string, string>();
        var unvisited = new List<string>();

        foreach (var vertex in graph)
        {
            distances[vertex.Key] = int.MaxValue;
            previous[vertex.Key] = null;
            unvisited.Add(vertex.Key);
        }

        distances[start] = 0;

        while (unvisited.Count > 0)
        {
            string current = null;
            foreach (var vertex in unvisited)
            {
                if (current == null || distances[vertex] < distances[current])
                    current = vertex;
            }

            unvisited.Remove(current);

            if (current == end)
                break;

            if (graph.ContainsKey(current))
            {
                foreach (var neighbor in graph[current])
                {
                    int distance = distances[current] + neighbor.Value;
                    if (distance < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = distance;
                        previous[neighbor.Key] = current;
                    }
                }
            }
        }

        if (previous[end] == null)
            return null;

        var path = new List<string>();
        string currentVertex = end;
        while (currentVertex != null)
        {
            path.Insert(0, currentVertex);
            currentVertex = previous[currentVertex];
        }

        return path;
    }
}

public class Program
{
    public static void Main()
    {
        var graph = new Dictionary<string, Dictionary<string, int>>()
        {
            { "Capinópolis", new Dictionary<string, int>(){ {"Centralina", 40}, {"Ituiutaba", 30} } },
            { "Ituiutaba", new Dictionary<string, int>(){ {"Capinópolis", 30}, {"Monte Alegre de Minas", 85}, {"Douradinhos", 90} } },
            { "Itumbiara", new Dictionary<string, int>(){ {"Centralina", 20}, {"Tupaciguara", 55} } },
            { "Centralina", new Dictionary<string, int>(){ {"Capinópolis", 40}, {"Monte Alegre de Minas", 75}, {"Itumbiara", 20} } },
            { "Monte Alegre de Minas", new Dictionary<string, int>(){ {"Douradinhos", 28}, {"Ituiutaba", 85}, {"Centralina", 75}, {"Tupaciguara", 44}, {"Uberlândia", 60} } },
            { "Tupaciguara", new Dictionary<string, int>(){ {"Monte Alegre de Minas", 44}, {"Itumbiara", 55}, {"Uberlândia", 60} } },
            { "Douradinhos", new Dictionary<string, int>(){ {"Ituiutaba", 90}, {"Monte Alegre de Minas", 28}, {"Uberlândia", 63} } },
            { "Uberlândia", new Dictionary<string, int>(){ {"Douradinhos", 63}, {"Monte Alegre de Minas", 60}, {"Tupaciguara", 60}, {"Araguari", 30}, {"Romaria", 78}, {"Indianópolis", 45} } },
            { "Araguari", new Dictionary<string, int>(){ {"Uberlândia", 30}, {"Cascalho Rico", 28}, {"Estrela do Sul", 34} } },
            { "Indianópolis", new Dictionary<string, int>(){ {"Uberlândia", 45}, {"São Juliana", 40} } },
            { "Cascalho Rico", new Dictionary<string, int>(){ {"Araguari", 28}, {"Grupiara", 32} } },
            { "Estrela do Sul", new Dictionary<string, int>(){ {"Araguari", 34}, {"Grupiara", 38}, {"Romaria", 27} } },
            { "Grupiara", new Dictionary<string, int>(){ {"Cascalho Rico", 32}, {"Estrela do Sul", 38} } },
            { "Romaria", new Dictionary<string, int>(){ {"Estrela do Sul", 27}, {"Uberlândia", 78}, {"São Juliana", 28} } },
            { "São Juliana", new Dictionary<string, int>(){ {"Indianópolis", 40}, {"Romaria", 28} } }
        };

        while (true)
        {
            Console.Clear();

            Console.WriteLine("Olá! Eu sou o seu agente de turismo virtual.");
            Console.WriteLine("Qual a sua cidade de destino? (ou digite 'sair' para encerrar)");

            string origem = Console.ReadLine();

            if (origem.ToLower() == "sair")
                break;

            Console.WriteLine();
            Console.WriteLine("Ótimo! Agora, para qual cidade você deseja ir?");

            string destino = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Calculando a melhor rota...");

            var dijkstra = new DijkstraAlgorithm(graph);
            var menorCaminho = dijkstra.FindShortestPath(origem, destino);

            if (menorCaminho != null)
            {
                int distancia = 0;

                Console.WriteLine();
                Console.WriteLine("Aqui está a sua rota de " + origem + " para " + destino + ":");

                for (int i = 0; i < menorCaminho.Count - 1; i++)
                {
                    Console.Write(menorCaminho[i] + " -> ");
                    distancia += graph[menorCaminho[i]][menorCaminho[i + 1]];
                }

                Console.WriteLine(menorCaminho[menorCaminho.Count - 1]);
                Console.WriteLine("Distância total percorrida: " + distancia + " km");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Desculpe, não há uma rota possível entre " + origem + " e " + destino + ". Talvez seja melhor escolher outro destino.");
            }

            Console.WriteLine();
            Console.WriteLine("Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }
}