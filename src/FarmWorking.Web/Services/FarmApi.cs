using System.Net.Http.Json;
using FarmWorking.Shared;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace FarmWorking.Web.Services;

public class FarmApi(HttpClient http)
{
    public Task<DashboardSummary?> Dashboard() => http.GetFromJsonAsync<DashboardSummary>("api/dashboard");
    public async Task<List<Farm>> Farms() => await http.GetFromJsonAsync<List<Farm>>("api/farms") ?? [];
    public Task<FarmDetail?> Farm(Guid id) => http.GetFromJsonAsync<FarmDetail>($"api/farms/{id}");
    public async Task AddFarm(Farm item) { var response = await http.PostAsJsonAsync("api/farms", item); response.EnsureSuccessStatusCode(); }
    public async Task UpdateFarm(Guid id, Farm item) { var response = await http.PutAsJsonAsync($"api/farms/{id}", item); response.EnsureSuccessStatusCode(); }
    public async Task DeleteFarm(Guid id) { var response = await http.DeleteAsync($"api/farms/{id}"); response.EnsureSuccessStatusCode(); }
    public Task<WorkNote?> Note(Guid farmId, Guid noteId) => http.GetFromJsonAsync<WorkNote>($"api/farms/{farmId}/notes/{noteId}");
    public async Task AddNote(Guid farmId, WorkNote item) { var response = await http.PostAsJsonAsync($"api/farms/{farmId}/notes", item); response.EnsureSuccessStatusCode(); }
    public async Task UpdateNote(Guid farmId, Guid noteId, WorkNote item) { var response = await http.PutAsJsonAsync($"api/farms/{farmId}/notes/{noteId}", item); response.EnsureSuccessStatusCode(); }
    public async Task DeleteNote(Guid farmId, Guid noteId) { var response = await http.DeleteAsync($"api/farms/{farmId}/notes/{noteId}"); response.EnsureSuccessStatusCode(); }
    public async Task AddExtraStep(Guid farmId, Guid processId, FarmProcessExtraStep step) { var response=await http.PostAsJsonAsync($"api/farms/{farmId}/processes/{processId}/extra-steps",step);response.EnsureSuccessStatusCode(); }
    public async Task DeleteExtraStep(Guid farmId, Guid processId, Guid stepId) { var response=await http.DeleteAsync($"api/farms/{farmId}/processes/{processId}/extra-steps/{stepId}");response.EnsureSuccessStatusCode(); }
    public async Task StartProcessRun(Guid farmId,Guid processId,StartFarmProcessRunRequest request){var response=await http.PostAsJsonAsync($"api/farms/{farmId}/processes/{processId}/runs",request);response.EnsureSuccessStatusCode();}
    public async Task SetRunStepCompleted(Guid farmId,Guid runId,Guid stepId,bool completed){var response=await http.PutAsync($"api/farms/{farmId}/runs/{runId}/steps/{stepId}?completed={completed}",null);response.EnsureSuccessStatusCode();}
    public async Task<List<FarmTransaction>> Transactions() => await http.GetFromJsonAsync<List<FarmTransaction>>("api/transactions") ?? [];
    public Task<FarmTransaction?> Transaction(Guid id) => http.GetFromJsonAsync<FarmTransaction>($"api/transactions/{id}");
    public async Task<List<FinanceTag>> Tags() => await http.GetFromJsonAsync<List<FinanceTag>>("api/tags") ?? [];
    public async Task AddTransaction(FarmTransaction item) { var response = await http.PostAsJsonAsync("api/transactions", item); response.EnsureSuccessStatusCode(); }
    public async Task UpdateTransaction(Guid id, FarmTransaction item) { var response = await http.PutAsJsonAsync($"api/transactions/{id}", item); response.EnsureSuccessStatusCode(); }
    public async Task DeleteTransaction(Guid id) { var response = await http.DeleteAsync($"api/transactions/{id}"); response.EnsureSuccessStatusCode(); }
    public Task<FinanceTag?> Tag(Guid id) => http.GetFromJsonAsync<FinanceTag>($"api/tags/{id}");
    public async Task AddTag(FinanceTag item) { var response = await http.PostAsJsonAsync("api/tags", item); response.EnsureSuccessStatusCode(); }
    public async Task UpdateTag(Guid id, FinanceTag item) { var response = await http.PutAsJsonAsync($"api/tags/{id}", item); response.EnsureSuccessStatusCode(); }
    public async Task DeleteTag(Guid id) { var response = await http.DeleteAsync($"api/tags/{id}"); response.EnsureSuccessStatusCode(); }
    public async Task<List<WorkProcess>> Processes() => await http.GetFromJsonAsync<List<WorkProcess>>("api/processes") ?? [];
    public Task<WorkProcess?> Process(Guid id) => http.GetFromJsonAsync<WorkProcess>($"api/processes/{id}");
    public async Task AddProcess(WorkProcess item) { var response = await http.PostAsJsonAsync("api/processes", item); response.EnsureSuccessStatusCode(); }
    public async Task UpdateProcess(Guid id, WorkProcess item) { var response = await http.PutAsJsonAsync($"api/processes/{id}", item); response.EnsureSuccessStatusCode(); }
    public async Task AssignProcess(Guid id, IReadOnlyCollection<Guid> farmIds) { var response = await http.PutAsJsonAsync($"api/processes/{id}/farms", new AssignProcessRequest { FarmIds = farmIds.ToList() }); response.EnsureSuccessStatusCode(); }
    public async Task DeleteProcess(Guid id) { var response = await http.DeleteAsync($"api/processes/{id}"); response.EnsureSuccessStatusCode(); }
    public async Task<List<Supply>> Supplies() => await http.GetFromJsonAsync<List<Supply>>("api/supplies") ?? [];
    public Task<Supply?> Supply(Guid id) => http.GetFromJsonAsync<Supply>($"api/supplies/{id}");
    public async Task AddSupply(Supply item) { var response = await http.PostAsJsonAsync("api/supplies", item); response.EnsureSuccessStatusCode(); }
    public async Task<string> UploadSupplyImage(IBrowserFile file)
    {
        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream(5 * 1024 * 1024);
        using var image = new StreamContent(stream);
        image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        content.Add(image, "file", file.Name);
        using var response = await http.PostAsync("api/supplies/image", content);
        response.EnsureSuccessStatusCode();
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("url").GetString()!;
    }
    public async Task UpdateSupply(Guid id, Supply item) { var response = await http.PutAsJsonAsync($"api/supplies/{id}", item); response.EnsureSuccessStatusCode(); }
    public async Task DeleteSupply(Guid id) { var response = await http.DeleteAsync($"api/supplies/{id}"); response.EnsureSuccessStatusCode(); }
    public async Task<List<SupplyPrice>> SupplyPrices(Guid supplyId)=>await http.GetFromJsonAsync<List<SupplyPrice>>($"api/supplies/{supplyId}/prices")??[];
    public async Task AddSupplyPrice(Guid supplyId,decimal price){var response=await http.PostAsJsonAsync($"api/supplies/{supplyId}/prices",new AddSupplyPriceRequest{Price=price});response.EnsureSuccessStatusCode();}
    public async Task DeleteSupplyPrice(Guid id){var response=await http.DeleteAsync($"api/supplies/prices/{id}");response.EnsureSuccessStatusCode();}
    public async Task<List<FarmSupplyEntry>> FarmSupplies(Guid farmId)=>await http.GetFromJsonAsync<List<FarmSupplyEntry>>($"api/farms/{farmId}/supplies")??[];
    public async Task ProvisionFarmSupply(Guid farmId,ProvisionFarmSupplyRequest item){var response=await http.PostAsJsonAsync($"api/farms/{farmId}/supplies",item);response.EnsureSuccessStatusCode();}
    public async Task UseFarmSupply(Guid farmId,UseFarmSupplyRequest item){var response=await http.PostAsJsonAsync($"api/farms/{farmId}/supplies/use",item);response.EnsureSuccessStatusCode();}
    public async Task<List<Worker>> Workers() => await http.GetFromJsonAsync<List<Worker>>("api/workers") ?? [];
    public Task<Worker?> Worker(Guid id) => http.GetFromJsonAsync<Worker>($"api/workers/{id}");
    public async Task AddWorker(Worker item) { var response = await http.PostAsJsonAsync("api/workers", item); response.EnsureSuccessStatusCode(); }
    public async Task UpdateWorker(Guid id, Worker item) { var response = await http.PutAsJsonAsync($"api/workers/{id}", item); response.EnsureSuccessStatusCode(); }
    public async Task DeleteWorker(Guid id) { var response = await http.DeleteAsync($"api/workers/{id}"); response.EnsureSuccessStatusCode(); }
    public async Task<List<WorkerPayroll>> Payroll(Guid farmId)=>await http.GetFromJsonAsync<List<WorkerPayroll>>($"api/farms/{farmId}/payroll")??[];
    public async Task AddWorkerWorkDay(Guid farmId,Guid workerId,AddWorkerWorkDayRequest item){var response=await http.PostAsJsonAsync($"api/farms/{farmId}/workers/{workerId}/work-days",item);response.EnsureSuccessStatusCode();}
    public async Task SettleWorkerPayroll(Guid farmId,Guid workerId,DateTime date){var response=await http.PostAsJsonAsync($"api/farms/{farmId}/workers/{workerId}/settle-payroll",new SettleWorkerPayrollRequest{SettlementDate=date});response.EnsureSuccessStatusCode();}
    public async Task<List<WorkType>> WorkTypes()=>await http.GetFromJsonAsync<List<WorkType>>("api/work-types")??[];
    public async Task AddWorkType(WorkType item){var response=await http.PostAsJsonAsync("api/work-types",item);response.EnsureSuccessStatusCode();}
    public async Task UpdateWorkType(Guid id,WorkType item){var response=await http.PutAsJsonAsync($"api/work-types/{id}",item);response.EnsureSuccessStatusCode();}
    public async Task DeleteWorkType(Guid id){var response=await http.DeleteAsync($"api/work-types/{id}");response.EnsureSuccessStatusCode();}
    public async Task<List<KnowledgeItem>> Knowledge(Guid? parentId=null)=>await http.GetFromJsonAsync<List<KnowledgeItem>>(parentId.HasValue?$"api/knowledge?parentId={parentId}":"api/knowledge")??[];
    public async Task CreateKnowledgeFolder(Guid? parentId,string name){var response=await http.PostAsJsonAsync("api/knowledge/folders",new CreateKnowledgeFolderRequest{ParentId=parentId,Name=name});response.EnsureSuccessStatusCode();}
    public async Task UploadKnowledgeFile(Guid? parentId,IBrowserFile file){using var content=new MultipartFormDataContent();using var stream=file.OpenReadStream(20*1024*1024);using var data=new StreamContent(stream);data.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);content.Add(data,"file",file.Name);if(parentId.HasValue)content.Add(new StringContent(parentId.Value.ToString()),"parentId");using var response=await http.PostAsync("api/knowledge/files",content);response.EnsureSuccessStatusCode();}
    public async Task RenameKnowledgeItem(Guid id,string name){var response=await http.PutAsJsonAsync($"api/knowledge/{id}",new RenameKnowledgeItemRequest{Name=name});response.EnsureSuccessStatusCode();}
    public async Task DeleteKnowledgeItem(Guid id){var response=await http.DeleteAsync($"api/knowledge/{id}");response.EnsureSuccessStatusCode();}
    public string KnowledgeContentUrl(Guid id,bool download=false)=>new Uri(http.BaseAddress!,$"api/knowledge/{id}/content{(download?"?download=true":"")}").ToString();
}
