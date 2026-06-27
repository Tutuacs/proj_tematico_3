using api.Data.Repository.Todo;
using api.Helpers;
using api.Model.Dtos.Todo;
using api.Model.Entities;

namespace api.Service.Todo;

public class TodoService(ITodoRepository todoRepository, ILogger<TodoService> logger) : ITodoService
{
    private readonly ITodoRepository _todoRepository = todoRepository;
    private readonly ApiLogger<TodoService> _logger = new(logger);

    public async Task<ServiceResponse<TodoDb>> CreateAsync(CreateTodoDto dto)
    {
        try
        {
            var todo = await _todoRepository.CreateAsync(dto);

            return new ServiceResponse<TodoDb>
            {
                Data = todo,
                Message = "Tarefa criada com sucesso",
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, "Erro ao criar tarefa: {Message}", ex.Message);
            return new ServiceResponse<TodoDb>
            {
                Data = null,
                Message = "Erro ao criar tarefa",
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ServiceResponse<TodoDb>> GetByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);

        if (todo == null)
        {
            return new ServiceResponse<TodoDb>
            {
                Data = null,
                Message = "Tarefa não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<TodoDb>
        {
            Data = todo,
            Message = "Tarefa encontrada",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<List<TodoDb>>> GetAllAsync()
    {
        var todos = await _todoRepository.GetAllAsync();

        return new ServiceResponse<List<TodoDb>>
        {
            Data = todos,
            Message = "Tarefas listadas com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<TodoDb>> UpdateAsync(int id, UpdateTodoDto dto)
    {
        var todoExiste = await _todoRepository.GetByIdAsync(id);
        if (todoExiste == null)
        {
            return new ServiceResponse<TodoDb>
            {
                Data = null,
                Message = "Tarefa não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var todo = await _todoRepository.UpdateAsync(id, dto);

        if (todo == null)
        {
            return new ServiceResponse<TodoDb>
            {
                Data = null,
                Message = "Tarefa não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<TodoDb>
        {
            Data = todo,
            Message = "Tarefa atualizada com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public async Task<ServiceResponse<object>> DeleteAsync(int id)
    {
        var todoExiste = await _todoRepository.GetByIdAsync(id);
        if (todoExiste == null)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Tarefa não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        var deleted = await _todoRepository.DeleteAsync(id);

        if (!deleted)
        {
            return new ServiceResponse<object>
            {
                Data = null,
                Message = "Tarefa não encontrada",
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }

        return new ServiceResponse<object>
        {
            Data = null,
            Message = "Tarefa deletada com sucesso",
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }
}