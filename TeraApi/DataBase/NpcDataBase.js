/*На сайте выложили базу, зачем напрягаться с дешифровкой игры когда можно взять у них:
* http://teradatabase.net/ru/npcs/
*/
var result = [];
var ii1 = 0;

function addToResult() {
    $('#NpcTable tbody tr').each(
        function (a) {
            el = {};
            el.name = $(this).find(" .dt-title b").text();
            var temp_str = $(this).find(" .dt-id").text();
            el.header = parseInt(temp_str.substr(0, 5));
            el.id = parseInt(temp_str.substr(5));
            el.level = parseInt($($(this).find(" .dt-level")[0]).text());
            el.type = ($($(this).find(" .grade")).text() =="Элитный" ? "Elite" : "Normal");
            el.hp = parseInt($($(this).find(" .dt-level")[1]).text());
            el.location = $($(this).find(" .dt-reward")).text();
            result.push(el);
        });
    ii1++;
    $($('.next.paginate_button')[0]).click();
    if (ii1 < 116) setTimeout(addToResult, 100);
}
addToResult()
$("body").text(result);