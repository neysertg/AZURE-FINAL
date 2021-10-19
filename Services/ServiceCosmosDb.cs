using AZURE_FINAL.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AZURE_FINAL.Services
{
    public class ServiceCosmosDb
    {
        //todo funciona con un client de cosmos
        //hemos creado una cuenta en un endpoint llamada cochescls
        DocumentClient client;
        String bbdd;
        String collection;
        public ServiceCosmosDb(IConfiguration configuration)
        {
            String endpoint = configuration["CosmosDb:endPoint"];
            String primarykey = configuration["CosmosDb:primaryKey"];
            this.bbdd = "AZURE";
            this.collection = "PeliculasCollection";
            this.client = new DocumentClient(new Uri(endpoint), primarykey);
        }
        public async Task CrearBbddPeliculaAsync()
        {
            Database bbdd = new Database() { Id = this.bbdd };
            await this.client.CreateDatabaseAsync(bbdd);
        }
        public async Task CrearColeccionPeliculasAsync()
        {
            DocumentCollection coleccion = new DocumentCollection() { Id = this.collection };
            //Factory es para recuperar de cosmos la base de datos
            await this.client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(this.bbdd), coleccion);
        }
        public async Task InsertarPelicula(Pelicula pelicula)
        {
            //recuperamos la URI para la coleccion donde ira el vehiculo
            Uri uri = UriFactory.CreateDocumentCollectionUri(this.bbdd, this.collection);
            await this.client.CreateDocumentAsync(uri, pelicula);
        }
        public List<Pelicula> GetPeliculas()
        {
            // debemos indicar el numero de peliculas a recuperar
            FeedOptions options = new FeedOptions() { MaxItemCount = -1 };
            String sql = "SELECT * FROM C"; // a todo lo llama 'c'
            Uri uri = UriFactory.CreateDocumentCollectionUri(this.bbdd, this.collection);
            IQueryable<Pelicula> consulta = this.client.CreateDocumentQuery<Pelicula>(uri, sql, options);
            return consulta.ToList();
        }

        public async Task<Pelicula> FindPeliculaAsyn(String id)
        {
            Uri uri = UriFactory.CreateDocumentUri(this.bbdd, this.collection, id);
            //lo que recupera es de la clase document

            Document document = await this.client.ReadDocumentAsync(uri);
            //este documento es un stream
            //guardamos en el objeto stream en memoria lo que recuperamos, para luego leerlo en memoria
            MemoryStream memory = new MemoryStream();
            using (var stream = new StreamReader(memory))
            {
                document.SaveTo(memory);
                memory.Position = 0;
                //deserializamos con JsonConvert
                Pelicula pelicula = JsonConvert.DeserializeObject<Pelicula>(await stream.ReadToEndAsync());
                return pelicula;
            }
        }
        public async Task ModificarPelicula(Pelicula pelicula)
        {
            Uri uri = UriFactory.CreateDocumentUri(this.bbdd, this.collection, pelicula.Id);
            await this.client.ReplaceDocumentAsync(uri, pelicula);
        }

        public async Task EliminarPelicula(String id)
        {
            Uri uri = UriFactory.CreateDocumentUri(this.bbdd, this.collection, id);
            await this.client.DeleteDocumentAsync(uri);
        }

        public List<Pelicula> BuscarPeliculas(String nombre)
        {
            FeedOptions options = new FeedOptions() { MaxItemCount = -1 };
            Uri uri = UriFactory.CreateDocumentCollectionUri(this.bbdd, this.collection);
            String sql = "select * from c where c.Nombre='" + nombre + "'";
            IQueryable<Pelicula> query = this.client.CreateDocumentQuery<Pelicula>(uri, sql, options);
            IQueryable<Pelicula> querylambda = this.client.CreateDocumentQuery<Pelicula>(uri, options)
                    .Where(z => z.Nombre == nombre);

            return query.ToList();
        }

        public List<Pelicula> CrearPeliculas()
        {
            List<Pelicula> peliculas = new List<Pelicula>() {
            new Pelicula
            {
                Id="70474403",Nombre="JOSE CARLOS", Ape_paterno="FLORES",
                Ape_materno= "APAZA", Curso = "TALLER DE PROYECTOS"
            },
             new Pelicula
            {
               Id="70457896",Nombre="DIEGO", Ape_paterno="QUISPE",
                Ape_materno= "PARIZACA", Curso = "FEP"
            }
            };
            return peliculas;

            //}
        }
    }
}