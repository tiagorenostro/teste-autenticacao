Para executar os projetos o docker deve estar instalado com a imagem do RabbitMQ configurada, abaixo está o comando docker para a imagem do rabbitmq:
# latest RabbitMQ 3.13
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management

E nos projetos Autenticacao.Api e Autenticacao.Consumidor, verificar o arquivo appsettings.json para ajustar as seguintes configurações:
- NLog_targets_fileName (autenticacao-api.log, autenticacao-consumidor.log)
- Configuracao_ConnectionString (autenticacao.db)

Onde deverá conter o caminho completo para o arquivos

Após os executar os procedimentos acima, executar os projetos Autenticacao.Api e Autenticacao.Consumidor
Foi desenvolvido uma api rest para poder executar o login, cadastro, ativação e alteração de senha, o front da aplicação é gerado através da lib Swagger.
E o projeto Consumidor é uma aplicação console que escuta filas do rabbitmq para efetuar os envios dos e-mails em background.