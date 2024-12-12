namespace WinFormsApp1
{
    public partial class GraphWork : Form
    {
        private TextBox verticesTextBox1;
        private TextBox edgesTextBox1;
        private TextBox verticesTextBox2;
        private TextBox edgesTextBox2;
        private TextBox edgesInputTextBox1;
        private TextBox edgesInputTextBox2;
        private Button submitButton;
        private PictureBox graphPictureBox1;
        private PictureBox graphPictureBox2;
        private PictureBox graphPictureBox3 = new PictureBox();
        private PictureBox graphPictureBox4 = new PictureBox();
        private TextBox fileInputTextBox1;
        private TextBox fileInputTextBox2;
        public GraphWork()
        {
            InitializeComponent();
            this.Size = new Size(900, 900); 
            verticesTextBox1 = new TextBox { PlaceholderText = "Введіть кількість вершин 1го графа", Location = new Point(10, 10), Size = new Size(210, 100) };
            edgesTextBox1 = new TextBox { PlaceholderText = "Введіть кількість ребер 1го графа", Location = new Point(10, 40), Size = new Size(210, 100) };
            edgesInputTextBox1 = new TextBox { PlaceholderText = "Введіть вершини 1го графа (id1 id2)", Location = new Point(10, 70), Size = new Size(210, 100) };
            verticesTextBox2 = new TextBox { PlaceholderText = "Введіть кількість вершин 2го графа", Location = new Point(10, 100), Size = new Size(210, 100) };
            edgesTextBox2 = new TextBox { PlaceholderText = "Введіть кількість ребер 2го графа", Location = new Point(10, 130), Size = new Size(210, 100) };
            edgesInputTextBox2 = new TextBox { PlaceholderText = "Введіть вершини 2го графа (id1 id2)", Location = new Point(10, 160), Size = new Size(210, 100) };
            submitButton = new Button { Text = "Submit", Location = new Point(460, 60), Size = new Size(150, 50) };
            fileInputTextBox1 = new TextBox { PlaceholderText = "Введіть ім'я файлу для першого графа", Location = new Point(260, 10), Size = new Size(250, 100) };
            fileInputTextBox2 = new TextBox { PlaceholderText = "Введіть ім'я файлу для другого графа", Location = new Point(560, 10), Size = new Size(250, 100) };
            graphPictureBox1 = new PictureBox { Location = new Point(10, 220), Size = new Size(300, 300), BorderStyle = BorderStyle.FixedSingle };
            graphPictureBox2 = new PictureBox { Location = new Point(320, 220), Size = new Size(300, 300), BorderStyle = BorderStyle.FixedSingle };
            graphPictureBox3 = new PictureBox { Location = new Point(10, 530), Size = new Size(300, 300), BorderStyle = BorderStyle.FixedSingle };
            graphPictureBox4 = new PictureBox { Location = new Point(320, 530), Size = new Size(300, 300) , BorderStyle = BorderStyle.FixedSingle };

            Label label1 = new Label { Text = "До видалення петель", Location = new Point(graphPictureBox3.Right + 10, graphPictureBox3.Top), AutoSize = true };
            Label label2 = new Label { Text = "Після видалення петель", Location = new Point(graphPictureBox1.Right + 10, graphPictureBox1.Top), AutoSize = true };

            this.Controls.Add(label1);
            this.Controls.Add(label2);


            submitButton.Click += (sender, e) =>
            {
                Graph graph1;
                Graph graph2;

                if (!string.IsNullOrEmpty(fileInputTextBox1.Text) && !string.IsNullOrEmpty(fileInputTextBox2.Text))
                {
                    graph1 = InputGraphFromFile(fileInputTextBox1.Text);
                    graph2 = InputGraphFromFile(fileInputTextBox2.Text);
                }
                else
                {

                    graph1 = InputGraphFromTextBoxes(verticesTextBox1, edgesTextBox1, edgesInputTextBox1);
                    graph2 = InputGraphFromTextBoxes(verticesTextBox2, edgesTextBox2, edgesInputTextBox2);
                }
                DrawGraph(graphPictureBox3, graph1);
                DrawGraph(graphPictureBox4, graph2);
                graph1.RemoveLoops();
                graph2.RemoveLoops();

                DrawGraph(graphPictureBox1, graph1);
                DrawGraph(graphPictureBox2, graph2);

                bool areEquivalent = CheckGraphsEquivalence(graph1, graph2);
                string equivalenceMessage = areEquivalent ? "Графи еквівалентні" : "Графи не еквівалентні";

                int centralVerticesCount1 = graph1.CountCentralVertices();
                int centralVerticesCount2 = graph2.CountCentralVertices();
                string centralVerticesMessage = $"Кількість центральних вершин в першому графі: {centralVerticesCount1}\nКількість центральних вершин в другому графі: {centralVerticesCount2}";

                MessageBox.Show($"{equivalenceMessage}\n{centralVerticesMessage}");
            };
            Controls.Add(graphPictureBox3);
            Controls.Add(graphPictureBox4);
            Controls.Add(verticesTextBox1);
            Controls.Add(edgesTextBox1);
            Controls.Add(edgesInputTextBox1);
            Controls.Add(verticesTextBox2);
            Controls.Add(edgesTextBox2);
            Controls.Add(edgesInputTextBox2);
            Controls.Add(submitButton);
            Controls.Add(graphPictureBox1);
            Controls.Add(graphPictureBox2);
            Controls.Add(fileInputTextBox1);
            Controls.Add(fileInputTextBox2);
        }

        public Graph Graph
        {
            get => default;
            set
            {
            }
        }

        private Graph InputGraphFromTextBoxes(TextBox verticesTextBox, TextBox edgesTextBox, TextBox edgesInputTextBox)
        {
            Graph graph = new Graph();

            int verticesCount = int.Parse(verticesTextBox.Text);
            for (int i = 0; i < verticesCount; i++)
            {
                graph.AddVertex(new Vertex(i));
            }

            string[] edges = edgesInputTextBox.Text.Split(' ');
            for (int i = 0; i < edges.Length; i += 2)
            {
                Vertex from = graph.Vertices[int.Parse(edges[i])];
                Vertex to = graph.Vertices[int.Parse(edges[i + 1])];
                graph.AddEdge(new Edge(from, to));
            }

            return graph;
        }

        private Graph InputGraphFromFile(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                MessageBox.Show($"Файл {filename} не знайдено. Будь ласка, перевірте назву файлу та спробуйте знову.");
                return new Graph();
            }

            Graph graph = new Graph();
            string[] lines = System.IO.File.ReadAllLines(filename);
            int verticesCount = int.Parse(lines[0]);
            for (int i = 0; i < verticesCount; i++)
            {
                graph.AddVertex(new Vertex(i));
            }

            int edgesCount = int.Parse(lines[1]);
            for (int i = 0; i < edgesCount; i++)
            {
                string[] verticesIds = lines[i + 2].Split(' ');
                Vertex from = graph.Vertices[int.Parse(verticesIds[0])];
                Vertex to = graph.Vertices[int.Parse(verticesIds[1])];
                graph.AddEdge(new Edge(from, to));
            }

            return graph;
        }

        private void DrawGraph(PictureBox pictureBox, Graph graph)
        {
            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                PointF center = new PointF(pictureBox.Width / 2, pictureBox.Height / 2);
                float radius = Math.Min(center.X, center.Y) - 50;

                List<Vertex> centralVertices = graph.Vertices.OrderBy(v => graph.GetEccentricity(v)).Take(graph.CountCentralVertices()).ToList();

                for (int i = 0; i < graph.Vertices.Count; i++)
                {
                    double angle = 2 * Math.PI * i / graph.Vertices.Count;
                    float x = center.X + radius * (float)Math.Cos(angle);
                    float y = center.Y + radius * (float)Math.Sin(angle);

                    if (centralVertices.Contains(graph.Vertices[i]))
                    {
                        g.FillEllipse(Brushes.Red, x - 15, y - 15, 30, 30);
                    }
                    else
                    {
                        g.FillEllipse(Brushes.Black, x - 15, y - 15, 30, 30);
                    }

                    Font vertexFont = new Font(SystemFonts.DefaultFont.FontFamily, 16);
                    g.DrawString(i.ToString(), vertexFont, Brushes.White, x - 15, y - 15);
                }

                foreach (Edge edge in graph.Edges)
                {
                    int from = edge.From.Id;
                    int to = edge.To.Id;

                    double angleFrom = 2 * Math.PI * from / graph.Vertices.Count;
                    float xFrom = center.X + radius * (float)Math.Cos(angleFrom);
                    float yFrom = center.Y + radius * (float)Math.Sin(angleFrom);

                    double angleTo = 2 * Math.PI * to / graph.Vertices.Count;
                    float xTo = center.X + radius * (float)Math.Cos(angleTo);
                    float yTo = center.Y + radius * (float)Math.Sin(angleTo);

                    if (from == to)
                    {
                        g.DrawEllipse(Pens.Black, xFrom - 20, yFrom - 20, 40, 40);
                    }
                    else
                    {
                        g.DrawLine(Pens.Black, xFrom, yFrom, xTo, yTo);
                    }
                }
            }

            pictureBox.Image = bitmap;
        }


        private bool CheckGraphsEquivalence(Graph graph1, Graph graph2)
        {
            if (graph1.Vertices.Count != graph2.Vertices.Count)
            {
                return false;
            }
            if (graph1.Edges.Count != graph2.Edges.Count)
            {
                return false;
            }

            int centralVerticesCount1 = graph1.CountCentralVertices();
            int centralVerticesCount2 = graph2.CountCentralVertices();

            if (centralVerticesCount1 != centralVerticesCount2)
            {
                return false;
            }

            return true;
        }
        
    }
}
