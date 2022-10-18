using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using HotChocolate;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Mutations {
    public class Mutations {
        private readonly List<CourseResult> _courses;
        public Mutations() => _courses = new List<CourseResult>();

        public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender) {
            CourseResult courseType = new CourseResult() {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };
            _courses.Add(courseType);
            await topicEventSender.SendAsync(nameof (Subscription.CourseCreated), courseType);
            return courseType;
        }
        public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender) {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == id);
            if (course == null) throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOURND"));
            course.Name = courseInput.Name;
            course.Subject = courseInput.Subject;
            course.InstructorId = courseInput.InstructorId;

            string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(updateCourseTopic, course);

            return course;
        }
        public bool DeleteCourse(Guid id) {
            return _courses.RemoveAll(c => c.Id == id) >= 1;
        }
    } 
}
