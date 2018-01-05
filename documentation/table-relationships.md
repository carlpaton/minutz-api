app.instance   // instance is the account table
- id
- username
- password
- type
- active
- subscription
- subscriptionDate
- logo - byte[]
- colour - string
- style - string
- allowInformal - bool
- notificationTypeId - int
- notificationRoleId - int
- reminderId

app.subscription
- id
- name
- description
- term
- cost

app.person  // association to a instance
- instanceId // this is to be a pipe delimeted collection of instances this is needed for the login details
- role 
- full name

app.notificationType
- id - int
- name - string

app.notificationRole
- id - int
- name - string

app.reminder
- id 
- name
- description
