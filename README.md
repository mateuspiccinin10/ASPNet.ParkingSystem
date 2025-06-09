# 🅿️ ParkingSystem

Sistema de gerenciamento de veículos em estacionamentos, desenvolvido em ASP.NET Core com autenticação via JWT.

---

## Funcionalidades

- Cadastro, edição, exclusão e listagem de *estacionamentos*
- Registro de *veículos* (Carro ou Moto)
- Registro de *entrada e saída de veículos*
- Autenticação com JWT para dois tipos de usuários:
  - admin: pode ativar/desativar usuários e registrar estacionamentos, veículos e entrada/saída
  - operator: pode registrar estacionamentos, veículos e entrada/saída
