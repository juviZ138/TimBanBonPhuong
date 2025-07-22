export type Member = {
  id: string;
  dateOfBirth: string;
  imageUrl: string;
  displayName: string;
  created: string;
  lastActive: string;
  description?: string;
  gender: string;
  city: string;
  country: string;
};

export type Photo = {
  id: number;
  url: string;
  publicId?: any;
  memberId: string;
};
