# Desafio backend *SuperBike*.
Solução para o desafio permitindo cadastrar motocicletas e aluguel para uso.

## 1º Clonar codigo fonte.
Execute a seguinte linha de comando: *"necessário git cli"*
```cmd
    git clone https://github.com/aasf86/SuperBike
```
## 2º Construir ambiente
No diretório raiz do projeto, executar a seguinte linha de comando: *"necessário docker"*
```cmd
    docker-compose up -d
```
---
## 3º Demonstração de uso dos serviços.
Uma vez construido o ambiente, na maquina hospedeira é possivel acessar os servicos.

- Solução SuperBike
    - SuperBike.Api: http://localhost:8081/swagger/index.html
        - *Responsável pela gestão de casdastros de motocicletas, usuários e alugueis*
    - SuperBike.Consumer: http://localhost:8083/swagger/index.html
        - *Responsável pelo consumo das mensagens enfileiradas no message broker*
    - SuperBike.Api.FileStorage: http://localhost:8082/swagger/index.html
        - *Responsável pela upload e download dos arquivos de imagem para a CNH*

- Ferramentas de infraestrutura
    - RabbitMQ: http://localhost:15672/#/
        - *Responsável pela gestão de fila*
    - Seq: http://localhost:81/#/
        - *Responsável pela gestão de logs da solução SuberBike*
    - PostgreSQL: port 5432
        - *Banco de dados relacional responsável pela retenção dos dados.*

### Vamos a demonstração. *Consulte a especificação da api aqui: http://localhost:8081/swagger/index.html*
Por padrão já temos um usuário Admin cadastrado para iniciar o cadastro de motocicletas.

- 1 Fazer login como Admin:
    - Login
        ```json
            // POST => http://localhost:8081/api/User/Login
            {"loginUserName": "admin@email.com", "password": "Admin*123456"}
        ```
    Agora com <strong>access token</strong> em mãos, adicione ele no cabeçalho de todas as requições para a gestão de cadastros de motocicletas.
    ```javascript
        header['Authorization'] = 'Bearer <access token>';
    ```

- 2 Crud de motocicletas.
    - Crud
        ```json        
            //POST => http://localhost:8081/api/Motorcycle
            //PUT => http://localhost:8081/api/Motorcycle
            //GET => http://localhost:8081/api/Motorcycle/{plate}
            //DELETE => http://localhost:8081/api/Motorcycle/{id}
        ```

- 3 Cadastrar usuário alugador/entregador.

    - 3/1 Cadastrar usuário primeiro, para depois cadastrar o alugador/entregador

        ```json        
            //POST => http://localhost:8081/api/User         
        ```

    - 3/2 Fazer login com o usuário que foi cadastrado acima
        ```json        
            //POST => http://localhost:8081/api/User/Login
        ```

        Agora com <strong>access token</strong> em mãos, adicione ele no cabeçalho de todas as requições para a gestão de cadastros de motocicletas.

        ```javascript
            header['Authorization'] = 'Bearer <access token>';
        ```    

    - 3/3 Cadastrar alugador/entregador(RenterDeliveryman)    
        ```json
            //POST => http://localhost:8081/api/Renter
        ```
- 4 Realizar o aluguel de uma motocicleta    
    - 4/1 Alugar
    ```json
        //POST => http://localhost:8081/api/Rent
    ```
    - 4/2 Consultar valor do aluguel
    ```json
        //GET => http://localhost:8081/api/Rent
    ```