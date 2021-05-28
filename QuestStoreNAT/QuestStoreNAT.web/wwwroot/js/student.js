function printNameStudent()
{
    var studentName = 'Jan Kowalski';
    var name = document.getElementById('nameStudent');
    name.textContent = studentName; 
}

function printCoolcoins()
{
    var coolcoins = 25;
    var numberOfCoolcoins = document.getElementById('coolcoins');
    numberOfCoolcoins.textContent = "Coolcoins : " + coolcoins; 
}

function printLevel()
{
    var level = 1;
    let numberOfLevel = document.getElementById('Level');
    var element = document.createElement("img");
    element.src("../images/level/first.jpg");
    Level.appendChild(element);
}

var landingContent = document.getElementById("artifact");

class QuestReward{
    constructor(title, cost, description){
        this.title = title;
        this.cost = cost;
        this.description = description;
    }
    printQuest(){
        let QuestReward = document.createElement('div');
        let Title = document.createElement('h3');
        let Cost = document.createElement('h3');
        let Description = document.createElement('p');

        var button = document.createElement("BUTTON");
        button.appendChild(document.createTextNode("USE"));
        //css
        QuestReward.className = "questreward";
        Title.className = "questreward__title";
        Cost.className = "questreward__cost";
        Description.className = "questreward__description";
        button.id = "deleteItem";
        //values
        Title.innerText = this.title;
        Cost.innerText = this.cost;
        Description.innerText = this.description;
        //creating proper div
        QuestReward.appendChild(Title);
        QuestReward.appendChild(Cost);
        QuestReward.appendChild(Description);
        QuestReward.appendChild(button);
        button.onclick = function(){ 
            QuestReward.remove();
            console.log(QuestReward.Description);
            printUsedRewards(QuestReward);
        }
        //adding quest to landing
        let landing__content = document.getElementById("artifact");
    
        landing__content.appendChild(QuestReward);
    }
}

class QuestRewardUsed{
    constructor(title, cost, description){
        this.title = title;
        this.cost = cost;
        this.description = description;
    }
    printQuestUsed(){
        let QuestReward = document.createElement('div');
        let Title = document.createElement('h3');
        let Cost = document.createElement('h3');
        let Description = document.createElement('p');
        
        //css
        QuestReward.className = "questreward";
        Title.className = "questreward__title";
        Cost.className = "questreward__cost";
        Description.className = "questreward__description";
        //values
        Title.innerText = this.title;
        Cost.innerText = this.cost;
        Description.innerText = this.description;
        //creating proper div
        QuestReward.appendChild(Title);
        QuestReward.appendChild(Cost);
        QuestReward.appendChild(Description);
        
        //adding quest to landing
        let landing__content = document.getElementById("usedArtifact");
    
        landing__content.appendChild(QuestReward);
    }
}

function modifyIndividual(){
    let cost = document.getElementsByClassName("questreward");
    for (i = 0; i < cost.length; i++) {
        cost[i].classList.add("questrewardIndividual");
    }
}
function printUsedRewards(QuestReward){
    let UsedQuestReward = [
        new QuestRewardUsed("Hint", "50cc", "One mentor advice."),
    ]
    UsedQuestReward.push(QuestReward)

    for (i = 0; i < UsedQuestReward.length; i++) {
        UsedQuestReward[i].printQuestUsed();
    }
    modifyIndividual();
}

function printAllIndividualRewards(){
    let Quests = [
    new QuestReward("Hint", "50cc", "One mentor advice."),
    new QuestReward("Remote work", "300cc", "You can spend a day in home office."),
    new QuestReward("Timetravel", "500cc", "Extend SI week assignment deadline by one day."),
    new QuestReward("Timetravel", "500cc", "Extend SI week assignment deadline by one day."),
    ]

    for (i = 0; i < Quests.length; i++) {
        Quests[i].printQuest();
    }
    modifyIndividual();
}


printNameStudent();
printCoolcoins();
