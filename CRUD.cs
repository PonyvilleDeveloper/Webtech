using System.Net;
using System.Text.Json.Serialization;

#nullable disable

namespace Webtech;
public delegate void Action(HttpListenerRequest req, HttpListenerResponse res); //Делегат реализации метода
internal struct CRUD {                                                          //Структура метода
    public string Name { get; set; }                                            //Название
    [JsonPropertyName("Url")]                                                   //След. поле в json называется "Url"
    public string RegUrl { get; set; }                                          //Рег. выражение для url
    public string Method { get; set; }                                          //Название Http метода
    public bool AuthRequired { get; set; }                                      //Флаг, отмечающий необходимость аутентифицированности пользователя для выполнения этого метода
    [JsonIgnore]                                                                //След. поле не записывается в json
    public Action act;                                                          //Ссылка на функцию, реализующую метод
    [JsonIgnore]
    public object role_attr;                                                    //Сохранение атрибута для использования ролей
}
