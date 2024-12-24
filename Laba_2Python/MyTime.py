class MyTime:
    def __init__(self, h = 0, m = 0, s = 0):
        total_seconds = (h * 3600) + (m * 60) + s
        self._set_from_seconds(total_seconds)

    def _set_from_seconds(self, total_seconds):
        sec_per_day = 60 * 60 * 24
        total_seconds %= sec_per_day

        if total_seconds < 0:
            total_seconds += sec_per_day

        self.hour = total_seconds // 3600
        self.minute = (total_seconds//60) % 60
        self.second = total_seconds % 60

    def __str__(self):
        return f"{self.hour:02} : {self.minute:02} : {self.second:02}"

    def time_since_midnight(self):
        return self.hour * 3600 + self.minute * 60 + self.second

    def add_seconds(self, seconds):
        total_seconds = self.time_since_midnight() + seconds
        self._set_from_seconds(total_seconds)

    def add_one_second(self):
        self.add_seconds(1)

    def add_one_minute(self):
        self.add_seconds(60)

    def add_one_hour(self):
        self.add_seconds(3600)

    def difference(self, other):
        return self.time_since_midnight() - other.time_since_midnight()
    
    def what_lesson(self):
        time_in_seconds = self.time_since_midnight()

        start_times = [ 
            8 * 3600,             
            9 * 3600 + 20 * 60,   
            9 * 3600 + 40 * 60,    
            11 * 3600,            
            11 * 3600 + 20 * 60,   
            12 * 3600 + 40 * 60,  
            13 * 3600,             
            14 * 3600 + 20 * 60,  
            14 * 3600 + 40 * 60,   
            16 * 3600,            
            16 * 3600 + 10 * 60,   
            17 * 3600 + 30 * 60,  
            17 * 3600 + 40 * 60,   
            19 * 3600             
        ]

        lessons = [
            "1-а пара", "перерва між 1-ю та 2-ю парами",
            "2-а пара", "перерва між 2-ю та 3-ю парами",
            "3-я пара", "перерва між 3-ю та 4-ю парами",
            "4-а пара", "перерва між 4-ю та 5-ю парами",
            "5-а пара", "перерва між 5-ю та 6-ю парами",
            "6-а пара", "пари вже скінчилися"
        ]

        for i in range(len(start_times)):
            if time_in_seconds < start_times[i]:
                return "пари ще не почалися" if i == 0 else lessons[i - 1]
            elif time_in_seconds < start_times[i+1]:
                return lessons[i]
            
        return "пари вже скінчилися"


a = MyTime(7, 59, 59)
print(a)
a.add_one_second()
print(a)