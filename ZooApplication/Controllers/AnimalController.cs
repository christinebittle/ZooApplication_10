﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using ZooApplication.Models;
using System.Web.Script.Serialization;


namespace ZooApplication.Controllers
{
    public class AnimalController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AnimalController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/animaldata/");
        }

        // GET: Animal/List
        public ActionResult List()
        {
            //objective: communicate with our animal data api to retrieve a list of animals
            //curl https://localhost:44324/api/animaldata/listanimals

           
            string url = "listanimals";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AnimalDto> animals = response.Content.ReadAsAsync<IEnumerable<AnimalDto>>().Result;
            //Debug.WriteLine("Number of animals received : ");
            //Debug.WriteLine(animals.Count());


            return View(animals);
        }

        // GET: Animal/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our animal data api to retrieve one animal
            //curl https://localhost:44324/api/animaldata/findanimal/{id}

            string url = "findanimal/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            AnimalDto selectedanimal = response.Content.ReadAsAsync<AnimalDto>().Result;
            Debug.WriteLine("animal received : ");
            Debug.WriteLine(selectedanimal.AnimalName);
            

            return View(selectedanimal);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Animal/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Animal/Create
        [HttpPost]
        public ActionResult Create(Animal animal)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(animal.AnimalName);
            //objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @animal.json https://localhost:44324/api/animaldata/addanimal 
            string url = "addanimal";

            
            string jsonpayload = jss.Serialize(animal);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

            
        }

        // GET: Animal/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findanimal/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AnimalDto selectedanimal = response.Content.ReadAsAsync<AnimalDto>().Result;
            return View(selectedanimal);
        }

        // POST: Animal/Update/5
        [HttpPost]
        public ActionResult Update(int id, Animal animal)
        {
            
            string url = "updateanimal/"+id;
            string jsonpayload = jss.Serialize(animal);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Animal/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findanimal/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AnimalDto selectedanimal = response.Content.ReadAsAsync<AnimalDto>().Result;
            return View(selectedanimal);
        }

        // POST: Animal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteanimal/"+id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}