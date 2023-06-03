[![Build microservices](https://github.com/FontysIPost/FIPost/actions/workflows/build-and-test-microservices.yml/badge.svg)](https://github.com/FontysIPost/FIPost/actions/workflows/build-and-test-microservices.yml)
[![SonarCloud analysis](https://github.com/FontysIPost/FIPost/actions/workflows/sonarcloud-scanner.yml/badge.svg)](https://github.com/FontysIPost/FIPost/actions/workflows/sonarcloud-scanner.yml)

<h3 align="middle">
<a href="https://github.com/FontysIPost/FIPost">Wiki</a>
<a>•</a>
<a href="https://github.com/FontysIPost/FIPost/blob/dev/.github/CONTRIBUTING.md">Contributing</a>
<a>•</a>
<a href="https://github.com/FontysIPost/FIPost/CONTACT.md">Contact</a>
</h3>

# 📜 ICA-Taskmaster

The aim of this project is to develop a mission management system for ICA that will improve the organisation's operational efficiency and effectiveness. 
The system should enable ICA staff to manage their missions more efficiently, track their progress and communicate with other team members and high-volume customers in a secure and streamlined manner.

## 🎯 Goals

* This project aims to improve the organisation's operational efficiency and effectiveness. Currently most of the administrative work is done manually.

* This project is designed with a microservices architecture to scale and manage the administrative post process of the entire Fontys organisation
  with over 10.000 personall. So this allows us to scale our software to support a large user base.

* To separate all concerns and keep the repositories SOLID each domain has been given each own codebase with its own repository.
  This is also to help maintain collaboration efficiently and opensource.

## ⚒️ Development

### 📐Stack
- **Node version:** 16x
- **NPM version:** 6.x
- **Frontend:** [SvelteKit](https://kit.svelte.dev/docs/introduction) - HTML/[TailwindCSS](https://tailwindcss.com/docs/installation)/JavaScript and [TypeScript](https://www.typescriptlang.org/docs/)
- **Backend:** [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-6.0.0-windows-x64-installer)

### 📁 Structure & services
- [./AgentService/](https://github.com/ICA-taskmaster/ICA-Taskmaster/tree/main/AgentService) Agent service
- [./EquipmentService/](https://github.com/ICA-taskmaster/ICA-Taskmaster/tree/main/EquipmentService) Equipment management
- [./MissionService/](https://github.com/ICA-taskmaster/ICA-Taskmaster/tree/main/MissionService)  Mission tracking and assignment manager

- postgreSQL in port 5432
- RabbitMQ in port 15672  


### 🏁 Getting started:
Clone the repository:
```sh
git clone --recursive https://github.com/ICA-taskmaster/ICA-Taskmaster.git
```
Navigate to `./K8S` folder and deplay all the services/deployments:

Create your own Kube secret for the postgres database password
```yml
kubectl create secret generic postgresql --from-literal=password=<password>
```

register ur own fake host in `C:\Windows\System32\drivers\etc` as 127.0.0.1 ica-taskmaster.org for ingress API gateway




## 🤝 Credits & Collaboration

Currently this project is being developed by semester 6 software student of the FHICT Spring 2024.
It is important that everything is well documented. 
every bit of help is appreciated and everyone who is willing to help out is welcome.

Check [CONTRIBUTING](https://github.com/FontysIPost/FIPost/blob/dev/.github/CONTRIBUTING.md) and [WIKI](https://github.com/FontysIPost/FIPost) for information.


## ✉️ Contact Us
Questions? [<ins>Contact us here </ins>](https://github.com/FIPost/docs/blob/master/CONTACT.md) !
