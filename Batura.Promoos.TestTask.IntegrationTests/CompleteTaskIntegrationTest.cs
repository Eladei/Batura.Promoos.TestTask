using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Batura.Promoos.TestTask.IntegrationTests;

// TODO Заготовка интеграционного теста
internal class CompleteTaskIntegrationTest
{
    //[Theory]
    public async Task Complete_WhenTaskIsNotCompleted_ShouldReturn200OkAndPublishMessage()
    {
        // act, assert
        /*
            - Вызвать url для регистрации задачи и зафиксировать вернувшийся Id задачи
            - Проверить корректность регистрации задачи
            - Вызвать url завершения задачи с полученным ранее Id
            - Проверить корректность инициализации значений IsCompleted, CompletedAt
            - Проверить факт отправки сообщения в шину сообщений
         */
    }
}
