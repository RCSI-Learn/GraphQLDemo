using System;
using System.Collections.Generic;

namespace GraphQLDemo.API.Schema.Queries {
    public enum Subject {
        Mathematics,
        Science,
        History
    }
    public class CourseType {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        [HotChocolate.GraphQLNonNullType]
        public InstructorType Instructor { get; set; }
        public IEnumerable<StudentType> Students { get; set; }
    }
}
