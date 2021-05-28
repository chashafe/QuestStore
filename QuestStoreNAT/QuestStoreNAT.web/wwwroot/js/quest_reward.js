var landingContent = document.getElementById("landing__content");

class QuestReward{
    constructor(title, cost, description){
        this.title = title;
        this.cost = cost;
        this.description = description;
    }
    printQuestReward(truth){
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
        let dividerh3 = document.getElementById("dividerh3");
        
        if(truth === "put after individual h3") {
            QuestReward.setAttribute("onclick","togglePopup()");
            landingContent.insertBefore(QuestReward, dividerh3);
        }
        else{
            QuestReward.setAttribute("onclick","togglePopupGroup()");
            landingContent.appendChild(QuestReward);
        }
    }
}

function modifyIndividual(){
    let cost = document.getElementsByClassName("questreward");
    for (i = 0; i < cost.length; i++) {
        cost[i].classList.add("questreward--individual");
    }
}

function modifyGroup(){
    let cost = document.getElementsByClassName("questreward");
    for (i = 0; i < cost.length; i++) {
        cost[i].classList.add("questreward--group");
    }
}

function printAllQuests(){
    let Quests = [
    new QuestReward("Spotter", "50cc", "Spot a major mistake in the assignment."),
    new QuestReward("Demo Master", "100cc", "Doing a demo for the class (side project, new technology, ...)."),
    new QuestReward("Screening", "100cc", "Taking part in the student screening process."),
    new QuestReward("Teacher", "400cc", "Organizing a workshop for other students."),
    new QuestReward("Wake up call", "300cc", "Attend 1 months without being late."),
    new QuestReward("New Me", "500-1000cc", "Set up a SMART goal accepted by a mentor, then achieve it."),
    new QuestReward("Best in field", "500cc", "Students choose the best project of the week. Selected team scores."),
    new QuestReward("Presenting FTW", "500cc", "Do a presentation on a meet-up."),
    new QuestReward("3D King", "500-1000cc", "Transform rougelike project to be playable in 3D."),
    ]

    for (i = 0; i < Quests.length; i++) {
        Quests[i].printQuestReward();
    }
}

function printAllIndividualRewards(truth){
    let Quests = [
    new QuestReward("Hint", "50cc", "One mentor advice."),
    new QuestReward("Remote work", "300cc", "You can spend a day in home office."),
    new QuestReward("Timetravel", "500cc", "Extend SI week assignment deadline by one day."),
    new QuestReward("First class", "1000cc", "1h one on one with the mentor of your choosing."),
    ]

    for (i = 0; i < Quests.length; i++) {
        Quests[i].printQuestReward(truth);
    }
    modifyIndividual();
}

function printAllGroupRewards(){
    let Quests = [
    new QuestReward("Lecture", "1000cc", "60 min workshop by a mentor(s) of the chosen topic."),
    new QuestReward("Backup", "1000cc", "Mentor joins a students' team for a one hour."),
    new QuestReward("Bookworm", "500cc", "Extra material for the current topic."),
    new QuestReward("Style it up", "5000cc", "All mentors should dress up as pirates (or just funny) for the day."),
    new QuestReward("Day trip", "5000cc", "The whole course goes to an off-school program instead for a day."),
    ]

    for (i = 0; i < Quests.length; i++) {
        Quests[i].printQuestReward();
    }
    modifyGroup();
}

//unused function - for education purposes only
function addH3Div(text){
    let wrapper = document.createElement("div");
    let header = document.createElement("h3");
    wrapper.className = "landing__content__wrapper";
    header.classList.add("landing__content--border", "landing__content--margin");
    header.innerText = text;
    wrapper.appendChild(header);

    let landing__content = document.getElementById("landing__content");
    let dividerh3 = document.getElementById("dividerh3");

    landing__content.insertBefore(wrapper, dividerh3);
}
