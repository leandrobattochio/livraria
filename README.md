# Nesse repositório de exemplo, com apenas dois endpoints bem simples (criar e obter livro), exemplifico como fazer o uso do padrão CQRS - sem uso do MediatR como muitos usam. 

O uso do MediatR para CQRS não é necessário, apenas acaba adicionando uma camada a mais no processo dificultando o entendimento do código e sua depuração. Objetos são instanciados sem necessidade.

Mostro também como aplicar o Result Pattern, evitando lançamento de exceções e mantendo os resultados consistentes em toda a aplicação. Utilizando também Repository Pattern e Unit of Work.

# Com relação aos testes de arquitetura, utilizei NetArchTest, definindo algumas regras:
- Todos os Commands/Queries devem ter uma classe de validação utlizando FluentValidations;
- Todos os Commands/Queries devem ter um Handler implementado;
- Todos os Repositórios devem ter implementações;
- A camada de Domínio não deve referenciar a camada de Aplicação;

Dessa forma, um comando faltando validação/handler, apesar do projeto compilar sem erros, não passaria pelos fluxos internos de CI/CD e jamais chegaria a produção.

# Testes

Testes de integração, utilizando TestContainers, subindo banco de dados no momento da execução dos testes utilizando docker, desta forma simulando o ambiente mais próximo possível de Produção.

Testes unitários para os Command/Query handlers.

# CI/CD

Pipeline CI/CD funcional com estágios de build/test. Geração de relatório de testes e porcentagem de cobertura. Estágio de deploy utilizando AWS Beanstalk.
